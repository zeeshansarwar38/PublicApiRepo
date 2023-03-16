using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using PublicApiRepo.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace PublicApiRepo
{
    public class PublicApiRepoTimerFunction
    {
        private IPublicAPIsDataService _publicAPIsDataService;
        private IApiLogService _apiLogService;
        private IBlobService _blobService;

        public PublicApiRepoTimerFunction(IPublicAPIsDataService publicAPIsDataService, IApiLogService apiLogService, IBlobService blobService) =>
                                                           (_publicAPIsDataService,_apiLogService,_blobService) = (publicAPIsDataService, apiLogService,blobService);

        [FunctionName("PublicApiRepoTimerFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer)
        {
            var logId = Guid.NewGuid().ToString();
            try
            {
                var response = await _publicAPIsDataService.GetAPIsDataAsync();
                await _apiLogService.Log(logId, "Success", response: response);
                await _blobService.SavePayloadAsync(logId, response);
            }
            catch (Exception ex)
            {
                logId = Guid.NewGuid().ToString();
                await _apiLogService.Log(logId, "Error occured", ex);
            }
        }
    }
}
