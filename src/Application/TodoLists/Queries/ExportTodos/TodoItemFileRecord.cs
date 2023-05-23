using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Entities;

namespace mentor_v1.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
