using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PublicApiRepo.Interfaces;
using PublicApiRepo.Services;
using System.Security.Cryptography;
using System.Net.Http.Json;
using System.Collections.Generic;
using PublicApiRepo.Validators;
using System.Collections;
using System.Linq;

namespace PublicApiRepo
{
    public class GetLogsFunction
    {
        private IApiLogService _apiLogService;
        public GetLogsFunction(IApiLogService apiLogService) =>
                                                           (_apiLogService) = (apiLogService);

        [FunctionName("GetLogsFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "logs/{From?}/{To?}")] HttpRequest req, string From, string To,
            ILogger log)
        {
            var validator = new LogQueryValidator();
            var query = new Models.LogQuery { To = To, From = From };
            var validationResult = await validator.ValidateAsync(query);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(e => new {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            try
            {
                var response = await _apiLogService.GetLogs(query);
                if(response == null || response.Count == 0)
                {
                    return new NotFoundObjectResult(new { error = "No logs available for the given duration" });
                }
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                await _apiLogService.Log(Guid.NewGuid().ToString(), "Error while getting logs", ex);
                return new NotFoundObjectResult(new { error = "No logs found"});
            }
        }
    }
}
