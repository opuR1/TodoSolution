using System;
using System.IO;
using Todo.Core;
using Xunit;

namespace Todo.Core.Tests
{
    public class TodoListJsonTests : IDisposable
    {
        private readonly string _tempFile;

        public TodoListJsonTests()
        {
            _tempFile = Path.Combine(Path.GetTempPath(), $"todo_{Guid.NewGuid():N}.json");
        }

        public void Dispose()
        {
            if (File.Exists(_tempFile)) File.Delete(_tempFile);
        }

        [Fact]
        public void Save_Then_Load_RestoresItems()
        {
            var list = new TodoList();
            list.Add("Buy milk");
            var done = list.Add("Read book");
            done.MarkDone();

            list.Save(_tempFile);
            Assert.True(File.Exists(_tempFile));

            var restored = new TodoList();
            restored.Load(_tempFile);

            Assert.Equal(2, restored.Count);
            Assert.Equal("Buy milk", restored.Items[0].Title);
            Assert.False(restored.Items[0].IsDone);
            Assert.Equal("Read book", restored.Items[1].Title);
            Assert.True(restored.Items[1].IsDone);
        }

        [Fact]
        public void Save_EmptyList_Load_GivesEmptyList()
        {
            var list = new TodoList();
            list.Save(_tempFile);

            var restored = new TodoList();
            restored.Add("temp");
            restored.Load(_tempFile);

            Assert.Equal(0, restored.Count);
        }

        [Fact]
        public void Load_MissingFile_Throws()
        {
            var restored = new TodoList();
            Assert.Throws<FileNotFoundException>(() =>
                restored.Load(Path.Combine(Path.GetTempPath(), "definitely_missing_file.json")));
        }
    }
}