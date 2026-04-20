using Azure.Data.Tables;
using AzureTables.Core;

namespace AzureTables.Infrastructure;

public class AzureTableTodoRepository : ITodoRepository
{
    private const string PartitionKey = "todos";
    private readonly TableClient _client;

    public AzureTableTodoRepository(string connectionString, string tableName = "todos")
    {
        _client = new TableClient(connectionString, tableName);
        _client.CreateIfNotExists();
    }

    public async Task<List<TodoItem>> GetAllAsync()
    {
        var result = new List<TodoItem>();
        await foreach (var entity in _client.QueryAsync<TodoEntity>())
        {
            result.Add(new TodoItem
            {
                Id = entity.RowKey,
                Title = entity.Title,
                Done = entity.Done
            });
        }
        return result;
    }

    public async Task SaveAsync(TodoItem item)
    {
        var entity = new TodoEntity
        {
            PartitionKey = PartitionKey,
            RowKey = item.Id,
            Title = item.Title,
            Done = item.Done
        };
        await _client.UpsertEntityAsync(entity);
    }

    public async Task DeleteAsync(TodoItem item)
    {
        await _client.DeleteEntityAsync(PartitionKey, item.Id);
    }
}
