using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OfficeOpenXml;

namespace mentor_v1.Application.TaxIncome.ExportExcelFileTaxIncomeCommands;
public class ExportExcelFileTaxIncome : IRequest<Stream>
{
}

public class ExportExcelFileTaxIncomeHandler : IRequestHandler<ExportExcelFileTaxIncome, Stream>
{
    public async Task<Stream> Handle(ExportExcelFileTaxIncome request, CancellationToken cancellationToken)
    {
        string[] columnNames = new string[] { "Muc_chiu_thue_From", "Thue_suat", "He_so_tru", "Muc_chiu_thue_To" };
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
