using Azure;
using Azure.Data.Tables;
using System.Collections.Concurrent;

namespace WebApplication1
{
    public class PublicApi : ITableEntity
    {
        public bool IsSuccessful { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}