using Azure;
using Azure.Data.Tables;
using System;
using System.Net.Http;
using System.Text;

namespace PublicApiRepo.Models
{
    public class Message : ITableEntity
    {
        private const string MessagePartionKey = "LogEntry"; // Setting this value to a const string because we will store limited log records because https://api.publicapis.org only have 1425 records

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string LogMessage { get; set; }
        public string Status { get; set; }

        public Message()
        {
        }

        public Message(string logId, string logMessage, Exception exception = null, HttpResponseMessage response = null)
        {
            StringBuilder messageBuilder = new StringBuilder(logMessage);
            messageBuilder.AppendLine(exception?.ToString());
            Status = response?.StatusCode.ToString() ?? "Failure";
            PartitionKey = MessagePartionKey;
            RowKey = logId;
            LogMessage = messageBuilder.ToString();
        }
    }
}
