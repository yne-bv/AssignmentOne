using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpenApisController : ControllerBase
    {
        private const string _connectionString = "UseDevelopmentStorage=true";
        private const string _tableName = "publicApis";
        private const string _containerName = "log-container";
        private const string _partitionKey = "publicApis";

        public OpenApisController()
        {
            
        }

        [HttpGet]
        public ActionResult<IEnumerable<Pageable<PublicApi>>> Get()
        {
            try
            {
                var tableClient = new TableClient(
                    _connectionString, 
                    _tableName);

                var entities = tableClient.Query<PublicApi>(filter: $"PartitionKey eq '{_partitionKey}'");

                return Ok(entities);
            }
            catch (RequestFailedException ex)
            {
                // Handle any exceptions that may occur
                return StatusCode((int)ex.Status, ex.Message);
            }
        }

        [HttpGet("{rowKey}")]
        public async Task<IActionResult> GetFileFromBlobStorage(string rowKey)
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(_connectionString);

                var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

                var blobClient = containerClient.GetBlobClient(rowKey + ".json");

                var memoryStream = new MemoryStream();
                await blobClient.DownloadToAsync(memoryStream);

                memoryStream.Position = 0;

                return new FileStreamResult(memoryStream, blobClient.GetProperties().Value.ContentType);
            }
            catch (RequestFailedException ex)
            {
                return StatusCode((int)ex.Status, ex.Message);
            }
        }
    }
}