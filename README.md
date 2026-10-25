# Todo.Core

Простая .NET-библиотека для работы со списком задач (add / remove / mark done)
с JSON-персистенцией и CI/CD-публикацией в GitHub Packages.

## CI/CD
- `CI / Build & Test` — restore, build, test, pack на каждый push/PR.
- `Publish package to GitHub Packages` — публикует `.nupkg` после успешного CI.

## Установка пакета

### 1. Добавьте источник GitHub Packages
```bash
dotnet nuget add source \
  "https://nuget.pkg.github.com/opuR1/index.json" \
  --name github \
  --username <GITHUB_USERNAME> \
  --password <PAT_с_scope_read:packages> \
  --store-password-in-clear-text
```

### 2. Установите пакет
```bash
dotnet add package Todo.Core --version 0.1.0 --source github
```

## Использование
```csharp
using Todo.Core;

var list = new TodoList();
var item = list.Add("Buy milk");
item.MarkDone();

list.Save("tasks.json");

var restored = new TodoList();
restored.Load("tasks.json");
```