# csharp-console-azure-tables

Agent-generiertes Beispiel: Azure Table Storage mit .NET 10.

## Konzept

Todo-Liste, die Einträge in **Azure Table Storage** speichert — komplett ohne Azure Portal, nur mit `az` CLI.

| Dienst | Tier | Kosten |
|---|---|---|
| Azure Storage Account (Standard LRS) | Free Tier | 0 € (5 GB frei) |
| Table Storage Transaktionen | Free Tier | 0 € (erste 10 000/Monat frei) |

## Projektstruktur

```
AzureTables.Core/           ← TodoItem, ITodoRepository
AzureTables.Infrastructure/ ← AzureTableTodoRepository (Azure.Data.Tables)
AzureTables/                ← Console App, Program.cs
AzureTables.Tests/          ← xUnit (FakeTodoRepository, kein Azure nötig)
```

## Azure-Ressourcen einrichten

```bash
az login
bash setup.sh
```

Das Skript gibt am Ende einen `export`-Befehl aus, der die Connection String als Umgebungsvariable setzt.

## App starten

```bash
export AZURE_STORAGE_CONNECTION_STRING='...'
dotnet run --project AzureTables
```

## Tests

```bash
dotnet test
```

Die Tests laufen ohne Azure-Verbindung (FakeTodoRepository).

## Aufräumen

```bash
bash teardown.sh
```
