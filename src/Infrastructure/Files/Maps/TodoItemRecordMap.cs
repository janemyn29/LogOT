using System.Globalization;
using mentor_v1.Application.TodoLists.Queries.ExportTodos;
using CsvHelper.Configuration;

namespace mentor_v1.Infrastructure.Files.Maps;

public class TodoItemRecordMap : ClassMap<TodoItemRecord>
{
    public TodoItemRecordMap()
    {
        AutoMap(CultureInfo.InvariantCulture);

        Map(m => m.Done).ConvertUsing(c => c.Done ? "Yes" : "No");
    }
}
