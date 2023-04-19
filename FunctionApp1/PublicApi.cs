using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionApp1
{
    public partial class PublicApiFunction
    {
        public class PublicApi : TableEntity
        {
            public PublicApi(DateTime timestamp, bool isSuccess)
            {
                PartitionKey = "publicApis";
                RowKey = Guid.NewGuid().ToString("n");
                Timestamp = timestamp;
                IsSuccessful = isSuccess;
            }

            public bool IsSuccessful { get; set; }
        }
    }
}
