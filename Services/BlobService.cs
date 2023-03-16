using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PublicApiRepo.Interfaces;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PublicApiRepo.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _publicAPIsBlobClient;

        public BlobService(BlobContainerClient publicAPIsBlobClient)
        {
            _publicAPIsBlobClient = publicAPIsBlobClient;
        }

        public async Task<BlobDownloadResult> DownloadAsync(string logId)
        {
            string blobName = $"{logId}.json";
            BlobClient blobClient = _publicAPIsBlobClient.GetBlobClient(blobName);
            BlobDownloadResult response = await blobClient.DownloadContentAsync();
            return response;
        }

        public async Task SavePayloadAsync(string logId, HttpResponseMessage response)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            await _publicAPIsBlobClient.CreateIfNotExistsAsync();
            string blobName = $"{logId}.json";
            BlobClient blobClient = _publicAPIsBlobClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(responseBody)));
        }
    }
}
