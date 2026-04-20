using AzureTables.Core;

namespace AzureTables.Tests;

public class TodoRepositoryTests
{
    private sealed class FakeTodoRepository : ITodoRepository
    {
        private readonly List<TodoItem> _items = [];

        public Task<List<TodoItem>> GetAllAsync() => Task.FromResult(_items.ToList());

        public Task SaveAsync(TodoItem item)
        {
            var existing = _items.FirstOrDefault(i => i.Id == item.Id);
            if (existing is not null) _items.Remove(existing);
            _items.Add(item);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TodoItem item)
        {
            _items.RemoveAll(i => i.Id == item.Id);
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Save_AddsNewItem()
    {
        var repo = new FakeTodoRepository();
        await repo.SaveAsync(new TodoItem { Title = "Test" });
        var items = await repo.GetAllAsync();
        Assert.Single(items);
    }

    [Fact]
    public async Task Save_UpdatesExistingItem()
    {
        var repo = new FakeTodoRepository();
        var item = new TodoItem { Title = "Test", Done = false };
        await repo.SaveAsync(item);
        item.Done = true;
        await repo.SaveAsync(item);
        var items = await repo.GetAllAsync();
        Assert.Single(items);
        Assert.True(items[0].Done);
    }

    [Fact]
    public async Task Delete_RemovesItem()
    {
        var repo = new FakeTodoRepository();
        var item = new TodoItem { Title = "Test" };
        await repo.SaveAsync(item);
        await repo.DeleteAsync(item);
        var items = await repo.GetAllAsync();
        Assert.Empty(items);
    }

    [Fact]
    public async Task GetAll_ReturnsAllItems()
    {
        var repo = new FakeTodoRepository();
        await repo.SaveAsync(new TodoItem { Title = "A" });
        await repo.SaveAsync(new TodoItem { Title = "B" });
        var items = await repo.GetAllAsync();
        Assert.Equal(2, items.Count);
    }

    [Fact]
    public async Task Delete_OnlyRemovesTargetItem()
    {
        var repo = new FakeTodoRepository();
        var keep = new TodoItem { Title = "Behalten" };
        var remove = new TodoItem { Title = "Löschen" };
        await repo.SaveAsync(keep);
        await repo.SaveAsync(remove);
        await repo.DeleteAsync(remove);
        var items = await repo.GetAllAsync();
        Assert.Single(items);
        Assert.Equal("Behalten", items[0].Title);
    }
}
