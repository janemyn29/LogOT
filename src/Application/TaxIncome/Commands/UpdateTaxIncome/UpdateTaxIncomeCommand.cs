using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace mentor_v1.Application.TaxIncome.Commands.CreateTaxIncome;
public class UpdateTaxIncomeCommand : IRequest
{
    public IFormFile file { get; set; }
}

public class UpdateTaxIncomeCommandHandler : IRequestHandler<UpdateTaxIncomeCommand>
{

    private readonly IApplicationDbContext _context;
    private readonly IWebHostEnvironment environment;

    public UpdateTaxIncomeCommandHandler(IApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        this.environment = environment;
    }

    public async Task<Unit> Handle(UpdateTaxIncomeCommand request, CancellationToken cancellationToken)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//sử dụng để đặt ngữ cảnh giấy phép sử dụng của gói ExcelPackage thành phi thương mại

        List<Guid> oldIdTaxIncome = new List<Guid>();

        //lấy id những tax cũ
        var oldTaxInCome = _context.Get<Domain.Entities.DetailTaxIncome>().Where(x => x.IsDeleted == false).OrderBy(x => x.Muc_chiu_thue_From).AsNoTracking().ToList();
        foreach (var emp in oldTaxInCome)
        {
            oldIdTaxIncome.Add(emp.Id);
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
                var listTaxIncome = new List<DetailTaxIncome>();

                for (int row = 2; row <= rowCount; row++)
                {
                    var col = 1;
                    try
                    {
                        if (worksheet.Cells[row, 4].Value != null)
                        {
                            var careateTax = new DetailTaxIncome
                            {
                                Muc_chiu_thue_From = double.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                Thue_suat = double.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                He_so_tru = double.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                Muc_chiu_thue_To = double.Parse(worksheet.Cells[row, col++].Value.ToString().Trim())
                            };
                            listTaxIncome.Add(careateTax);
                        }
                        else
                        {
                            var careateTax = new DetailTaxIncome
                            {
                                Muc_chiu_thue_From = double.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                Thue_suat = double.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                He_so_tru = double.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                            };
                            listTaxIncome.Add(careateTax);
                        }
                    }
                    catch (Exception ex)
                    {
                        await stream.DisposeAsync();
                        throw new Exception($"Error at line {row}, Column Name: {worksheet.Cells[1, col - 1].Value}");
                    }
                }

                _context.Get<Domain.Entities.DetailTaxIncome>().AddRange(listTaxIncome);

                //set isDelete = true của những tax cũ vừa lấy
                if (oldIdTaxIncome.Count != 0)
                {
                    foreach (var emp in oldIdTaxIncome)
                    {
                        var taxIncome = await _context.Get<Domain.Entities.DetailTaxIncome>().Where(x => x.Id.Equals(emp)).FirstOrDefaultAsync();
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
