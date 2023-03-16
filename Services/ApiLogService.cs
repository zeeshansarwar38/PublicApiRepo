using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using PublicApiRepo.Interfaces;
using PublicApiRepo.Models;
using PublicApiRepo.Utils;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PublicApiRepo.Services
{
    public class ApiLogService: IApiLogService
    {
        private readonly TableClient _apiLogsTableClient;

        public ApiLogService(TableClient apiLogsTableClient)
        {
            _apiLogsTableClient = apiLogsTableClient;
        }

        public async Task<List<Message>> GetLogs(LogQuery query)
        {
            AsyncPageable<Message> results = _apiLogsTableClient.QueryAsync<Message>(item => item.Timestamp >= Convert.ToDateTime(query.From).StartOfDay() && item.Timestamp <= Convert.ToDateTime(query.To).EndOfDay());
            List<Message> messages = new List<Message>();
            await foreach (var entity in results)
            {
                messages.Add(entity);
            }
            return messages;
        }

        public async Task Log(string logId, string message, Exception exception=null, HttpResponseMessage response = null)
        {
            var logEntity = new Message(logId, message, exception, response);
            _apiLogsTableClient.CreateIfNotExists();
            await _apiLogsTableClient.AddEntityAsync(logEntity);
        }
    }
}
