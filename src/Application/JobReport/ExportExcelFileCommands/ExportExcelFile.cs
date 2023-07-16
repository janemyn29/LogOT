
using AutoMapper;
using Azure;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OfficeOpenXml;

namespace mentor_v1.Application.JobReport.ExportExcelFile;
public class ExportExcelFile : IRequest<Stream>
{
    public Guid Id { get; set; }
}

public class ExportExcelFileHandler : IRequestHandler<ExportExcelFile, Stream>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public ExportExcelFileHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<Stream> Handle(ExportExcelFile request, CancellationToken cancellationToken)
    {
        var jobReport = _applicationDbContext.Get<Domain.Entities.JobReport>()
                            .Include(x => x.ExcelContracts.Where(x => x.IsDeleted == false))
                            .Include(x => x.ExcelEmployeeQuits.Where(x => x.IsDeleted == false))
                            .Where(x => x.IsDeleted == false && x.Id == request.Id).AsNoTracking().FirstOrDefault();



        ExcelPackage.LicenseContext = LicenseContext.Commercial;
        var stream = new MemoryStream();

        using (var package = new ExcelPackage(stream))
        {
            //set data
            if (jobReport.ExcelContracts.ToList().Count != 0)
            {
                string[] columnNames = new string[] { "ContractCode", "StartDate", "EndTime", "EmployeeName", "ContractStatus", "Action", "ActionDate" };
                string header = string.Empty;

                foreach (var column in columnNames)
                {
                    header += column + ",";
                }

                var worksheet = package.Workbook.Worksheets.Add("ExcelContracts");
                worksheet.Cells.LoadFromText(header);

                int row = 2;
                foreach (var item in jobReport.ExcelContracts.ToList())
                {                
                    worksheet.Cells[row, 1].Value = item.ContractCode;
                    worksheet.Cells[row, 2].Value = item.StartDate.Date.ToString();
                    worksheet.Cells[row, 3].Value = item.EndTime.Date.ToString();
                    worksheet.Cells[row, 4].Value = item.EmployeeName;
                    worksheet.Cells[row, 5].Value = item.ContractStatus;
                    worksheet.Cells[row, 6].Value = item.Action;
                    worksheet.Cells[row, 7].Value = item.ActionDate.Date.ToString();
                    row++;
                }
                worksheet.Cells.AutoFitColumns();
            }
            else
            {
                string[] columnNames = new string[] { "FullName", "Username", "Email", "WorkStatus", "ActionType", "ActionDate"};
                string header = string.Empty;

                foreach (var column in columnNames)
                {
                    header += column + ",";
                }

                var worksheet = package.Workbook.Worksheets.Add("ExcelEmployeeQuits");
                worksheet.Cells.LoadFromText(header);
                int row = 2;
                foreach (var item in jobReport.ExcelEmployeeQuits.ToList()) 
                {                  
                    worksheet.Cells[row, 1].Value = item.FullName;
                    worksheet.Cells[row, 2].Value = item.Username;
                    worksheet.Cells[row, 3].Value = item.Email;
                    worksheet.Cells[row, 4].Value = item.WorkStatus;
                    worksheet.Cells[row, 5].Value = item.ActionType;
                    worksheet.Cells[row, 6].Value = item.ActionDate.ToString();
                    row++;
                }
                worksheet.Cells.AutoFitColumns();             
            }
            await package.SaveAsync();
        }
        stream.Position = 0;
        return stream;
    }
}
