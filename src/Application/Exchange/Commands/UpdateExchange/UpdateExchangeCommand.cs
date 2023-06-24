using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace mentor_v1.Application.Exchange.Commands.UpdateExchange;
public class UpdateExchangeCommand : IRequest
{
    public IFormFile file { get; set; }
}

public class UpdateExchangeCommandHandler : IRequestHandler<UpdateExchangeCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IWebHostEnvironment environment;

    public UpdateExchangeCommandHandler(IApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        this.environment = environment;
    }

    public async Task<Unit> Handle(UpdateExchangeCommand request, CancellationToken cancellationToken)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//sử dụng để đặt ngữ cảnh giấy phép sử dụng của gói ExcelPackage thành phi thương mại

        List<Guid> oldIdExchange = new List<Guid>();

        //lấy id những Exchange cũ
        var oldTaxInCome = _context.Get<Domain.Entities.Exchange>().Where(x => x.IsDeleted == false).OrderBy(x => x.Muc_Quy_Doi_To).AsNoTracking().ToList();
        foreach (var emp in oldTaxInCome)
        {
            oldIdExchange.Add(emp.Id);
        }

        using (var stream = new MemoryStream())
        {
            await request.file.CopyToAsync(stream);
            using (var package = new ExcelPackage(stream))
            {
                //Kiểm tra trang tính có null ko
                var worksheet = package.Workbook.Worksheets[0];
                //Điếm số hàng có giá trị
                int rowCount = worksheet.Dimension.Rows;
                var listExchange = new List<Domain.Entities.Exchange>();

                for (int row = 2; row <= rowCount; row++)
                {
                    var col = 1;
                    try
                    {
                        if (worksheet.Cells[row, 1].Value != null)
                        {
                            var careateTax = new Domain.Entities.Exchange
                            {
                                Muc_Quy_Doi_To = double.Parse(worksheet.Cells[row, col++].Value.ToString().Trim()),
                                Giam_Tru = double.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                Thue_Suat = double.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                Muc_Quy_Doi_From = double.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim())
                            };
                            listExchange.Add(careateTax);
                        }
                        else
                        {
                            var careateTax = new Domain.Entities.Exchange
                            {
                                Giam_Tru = double.Parse(worksheet.Cells[row, 2].Value.ToString()!.Trim()),
                                Thue_Suat = double.Parse(worksheet.Cells[row, 3].Value.ToString()!.Trim()),
                                Muc_Quy_Doi_From = double.Parse(worksheet.Cells[row, 4].Value.ToString()!.Trim())
                            };
                            listExchange.Add(careateTax);
                        }
                    }
                    catch (Exception ex)
                    {
                        await stream.DisposeAsync();
                        throw new Exception($"Error at line {row}, Column Name: {worksheet.Cells[1, col - 1].Value}");
                    }
                }

                _context.Get<Domain.Entities.Exchange>().AddRange(listExchange);

                //set isDelete = true của những tax cũ vừa lấy
                if (oldIdExchange.Count != 0)
                {
                    foreach (var emp in oldIdExchange)
                    {
                        var taxIncome = await _context.Get<Domain.Entities.Exchange>().Where(x => x.Id.Equals(emp)).FirstOrDefaultAsync();
                        taxIncome.IsDeleted = true;
                    }
                }

                if (await _context.SaveChangesAsync(cancellationToken) == 0)
                    throw new Exception("Cập nhật Tax thất bại.");

                return Unit.Value;
            }
        }
    }
}
