using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FunctionApp1
{
    public partial class PublicApiFunction
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private string _uri = "https://api.publicapis.org";
        private string _endpoint = "/random?auth=null";

        [FunctionName("PublicApi")]
        public async Task Run(
            [TimerTrigger("0 */1 * * * *")]TimerInfo myTimer,
            [Table("publicApis")] IAsyncCollector<PublicApi> collector,
            IBinder binder,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
            var publicApi = new PublicApi(DateTime.UtcNow, true);
            var response = await _httpClient.GetAsync(_uri + _endpoint);

            var outputBlob = await binder.BindAsync<Stream>(
                new BlobAttribute($"log-container/{publicApi.RowKey}.json")
                {
                    Connection = "AzureWebJobsStorage",
                    Access = FileAccess.Write
                });

            if (response.IsSuccessStatusCode)
            {
                publicApi.IsSuccessful = response.IsSuccessStatusCode;
                byte[] data = await response.Content.ReadAsByteArrayAsync();
                var newStream = new MemoryStream(data);
                await newStream.CopyToAsync(outputBlob);
            }

            await collector.AddAsync(publicApi);
        }
    }
}
