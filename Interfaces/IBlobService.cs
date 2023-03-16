using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PublicApiRepo.Interfaces
{
    public interface IBlobService
    {
        Task SavePayloadAsync(string logId, HttpResponseMessage response);
        Task<BlobDownloadResult> DownloadAsync(string logId);
    }
}
