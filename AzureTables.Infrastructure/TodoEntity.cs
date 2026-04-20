using Azure;
using Azure.Data.Tables;

namespace AzureTables.Infrastructure;

internal class TodoEntity : ITableEntity
{
    public string PartitionKey { get; set; } = "todos";
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public string Title { get; set; } = string.Empty;
    public bool Done { get; set; }
}
