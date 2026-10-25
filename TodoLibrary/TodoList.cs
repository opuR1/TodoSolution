using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Todo.Core
{
    public class TodoList
    {
        private readonly List<TodoItem> _items = new();

        public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();

        public int Count => _items.Count;

        public TodoItem Add(string title)
        {
            var item = new TodoItem(title);
            _items.Add(item);
            return item;
        }

        public bool Remove(Guid id) => _items.RemoveAll(i => i.Id == id) > 0;

        public IEnumerable<TodoItem> Find(string substring) =>
            _items.Where(i =>
                i.Title.Contains(substring ?? string.Empty, StringComparison.OrdinalIgnoreCase));


        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        public void Save(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path required", nameof(path));

            var json = JsonSerializer.Serialize(_items, JsonOptions);
            File.WriteAllText(path, json);
        }

        public void Load(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path required", nameof(path));

            if (!File.Exists(path))
                throw new FileNotFoundException("File not found", path);

            var json = File.ReadAllText(path);


            var dtos = JsonSerializer.Deserialize<List<TodoItemDto>>(json) ?? new List<TodoItemDto>();

            _items.Clear();
            foreach (var dto in dtos)
            {
                var item = new TodoItem(dto.Title ?? string.Empty);
                if (dto.IsDone) item.MarkDone();
                _items.Add(item);
            }
        }

        private sealed class TodoItemDto
        {
            public string? Title { get; set; }
            public bool IsDone { get; set; }
        }
    }
}