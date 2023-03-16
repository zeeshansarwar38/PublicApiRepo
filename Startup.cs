using Azure.Data.Tables;
using Azure.Storage.Blobs;
using PublicApiRepo.Interfaces;
using PublicApiRepo.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

[assembly: FunctionsStartup(typeof(PublicApiRepo.Startup))]

namespace PublicApiRepo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration["AzureWebJobsStorage"];
            var blobServiceClient = new BlobServiceClient(connectionString);
            var publicAPIsBlobClient = blobServiceClient.GetBlobContainerClient("public-apis-payloads");
            builder.Services.AddSingleton(publicAPIsBlobClient);

            var tableClient = new TableClient(connectionString, "ApiLogs");
            builder.Services.AddSingleton(tableClient);

            builder.Services.AddHttpClient<PublicAPIsDataService>("publicApiClient", client =>
            {
                // Set the base address of the named client.
                client.BaseAddress = new Uri("https://api.publicapis.org/");
            })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            builder.Services.AddSingleton<IPublicAPIsDataService, PublicAPIsDataService>();
            builder.Services.AddSingleton<IApiLogService, ApiLogService>();
            builder.Services.AddSingleton<IBlobService, BlobService>();
        }
    }
}
