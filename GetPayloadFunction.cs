using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using System.Reflection.Metadata;
using PublicApiRepo.Interfaces;

namespace PublicApiRepo
{
    public class GetPayloadFunction
    {
        private IBlobService _blobService;
        private IApiLogService _apiLogService;
        public GetPayloadFunction(IBlobService blobService, IApiLogService apiLogService) =>
                                                           (_blobService, _apiLogService) = (blobService, apiLogService);

        [FunctionName("GetPayloadFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "log/{logId?}/payload")] HttpRequest req, string logId, ILogger log)
        {
            try
            {
                if (string.IsNullOrEmpty(logId)) {
                    return new BadRequestObjectResult(new { error = "'logId' is required", field = "logId" });
                }
                var response = await _blobService.DownloadAsync(logId);              
                return new FileContentResult(response.Content.ToArray(), "application/json");
            }
            catch (Exception ex)
            {
                await _apiLogService.Log(Guid.NewGuid().ToString(), "Error while downloading payload for logId: " + logId, ex);
                return new NotFoundObjectResult(new { error = "No payload found" });
            }
        }
    }
}
