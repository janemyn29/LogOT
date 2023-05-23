using System.Globalization;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.TodoLists.Queries.ExportTodos;
using mentor_v1.Infrastructure.Files.Maps;
using CsvHelper;

namespace mentor_v1.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
    public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Configuration.RegisterClassMap<TodoItemRecordMap>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }
}
