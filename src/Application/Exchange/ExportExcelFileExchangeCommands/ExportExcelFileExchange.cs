using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OfficeOpenXml;

namespace mentor_v1.Application.Exchange.ExportExcelFileCommands;
public class ExportExcelFileExchange : IRequest<Stream>
{
    
}

public class ExportExcelFileHandler : IRequestHandler<ExportExcelFileExchange, Stream>
{
    public async Task<Stream> Handle(ExportExcelFileExchange request, CancellationToken cancellationToken)
    {
        string[] columnNames = new string[] { "Muc_Quy_Doi_To", "Giam_Tru", "Thue_Suat", "Muc_Quy_Doi_From" };
        string header = string.Empty;

        foreach (var column in columnNames)
        {
            header += column + ",";
        }

        var stream = new MemoryStream();

        using (var package = new ExcelPackage(stream))
        {
            var worksheet = package.Workbook.Worksheets.Add("Exchange");
            worksheet.Cells.LoadFromText(header);

            await package.SaveAsync();
        }

        stream.Position = 0;

        return stream;
    }
}
