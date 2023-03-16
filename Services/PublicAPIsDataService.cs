using PublicApiRepo.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PublicApiRepo.Services
{
    public sealed class PublicAPIsDataService : IPublicAPIsDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PublicAPIsDataService(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;


        public async Task<HttpResponseMessage> GetAPIsDataAsync()
        {
            var _httpClient = _httpClientFactory.CreateClient("publicApiClient");
            var response = await _httpClient.GetAsync($"random?auth=null");
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
