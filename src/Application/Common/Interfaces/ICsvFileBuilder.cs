using mentor_v1.Application.TodoLists.Queries.ExportTodos;

namespace mentor_v1.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
