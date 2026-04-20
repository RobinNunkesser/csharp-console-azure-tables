using AzureTables.Core;
using AzureTables.Infrastructure;

var connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING")
    ?? throw new InvalidOperationException(
        "AZURE_STORAGE_CONNECTION_STRING nicht gesetzt. Bitte setup.sh ausführen.");

ITodoRepository repository = new AzureTableTodoRepository(connectionString);

Console.WriteLine("Azure Table Storage – Todo Demo");
Console.WriteLine("================================");

// Einträge speichern
var item1 = new TodoItem { Title = "Azure CLI einrichten" };
var item2 = new TodoItem { Title = "Table Storage ausprobieren" };
var item3 = new TodoItem { Title = "Eintrag löschen üben" };

await repository.SaveAsync(item1);
await repository.SaveAsync(item2);
await repository.SaveAsync(item3);
Console.WriteLine("Drei Einträge gespeichert.\n");

// Alle lesen
var todos = await repository.GetAllAsync();
Console.WriteLine($"Alle Todos ({todos.Count}):");
foreach (var todo in todos)
    Console.WriteLine($"  [ ] {todo.Title}");

// Aktualisieren
item1.Done = true;
await repository.SaveAsync(item1);
Console.WriteLine("\nErsten Eintrag als erledigt markiert.");

// Löschen
await repository.DeleteAsync(item3);
Console.WriteLine("Dritten Eintrag gelöscht.\n");

// Finaler Zustand
todos = await repository.GetAllAsync();
Console.WriteLine($"Finaler Zustand ({todos.Count}):");
foreach (var todo in todos)
    Console.WriteLine($"  [{(todo.Done ? "x" : " ")}] {todo.Title}");

// Aufräumen
foreach (var todo in todos)
    await repository.DeleteAsync(todo);
Console.WriteLine("\nAlle Einträge bereinigt.");
