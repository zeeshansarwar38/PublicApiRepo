using Azure.Data.Tables;
using PublicApiRepo.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PublicApiRepo.Interfaces
{
    public interface IApiLogService
    {
        Task Log(string logId, string message, Exception exception = null, HttpResponseMessage response = null);

        Task<List<Message>> GetLogs(LogQuery query);

    }
}
