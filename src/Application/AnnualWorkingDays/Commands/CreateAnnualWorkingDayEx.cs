using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace mentor_v1.Application.AnnualWorkingDays.Commands;
public class CreateAnnualWorkingDayEx : IRequest<string>
{
    public IFormFile file { get; set; }
}

public class CreateAnnualWorkingDayExCommandHandler : IRequestHandler<CreateAnnualWorkingDayEx, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IWebHostEnvironment environment;


    public CreateAnnualWorkingDayExCommandHandler(IApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        this.environment = env;
    }

    public async Task<string> Handle(CreateAnnualWorkingDayEx request, CancellationToken cancellationToken)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//sử dụng để đặt ngữ cảnh giấy phép sử dụng của gói ExcelPackage thành phi thương mại
        try
        {
           using (var stream = new MemoryStream())
            {
                await request.file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    //Kiểm tra trang tính có null ko
                    var worksheet = package.Workbook.Worksheets[0];
                    //Điếm số hàng có giá trị
                    int rowCount = worksheet.Dimension.Rows;
                    var annualWorkingDays = new List<AnnualWorkingDay>();
                    var configDay = _context.ConfigDays.FirstOrDefault();
                    var coefficientList = _context.Coefficients;

                    for (int row = 2; row <= rowCount; row++) // Bắt đầu từ hàng thứ 2 để bỏ qua tiêu đề
                    {
                        var dayCell = worksheet.Cells[row, 1].Value;
                        var typeCell = worksheet.Cells[row, 2].Value;

                        if (dayCell != null && typeCell == null)
                        {
                            DateTime day;

                            if (DateTime.TryParse(dayCell.ToString(), out day))
                            {
                                if (_context.AnnualWorkingDays.Any(x => x.Day.Date == day.Date))
                                {
                                    continue; // Bỏ qua hàng này và chuyển sang hàng tiếp theo
                                }
                                ShiftType shiftType;
                                TypeDate typeDate;
                                

                                // Tự động tính toán hệ số lương và loại ngày dựa trên ngày
                                //Ngày t7
                                if (day.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    typeDate = TypeDate.Saturday;
                                    shiftType = configDay.Saturday;
                                }
                                else if (day.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    typeDate = TypeDate.Sunday;
                                    shiftType = configDay.Sunday;
                                }
                                else
                                {
                                    typeDate = TypeDate.Normal;
                                    shiftType = configDay.Normal;
                                }
                                
                                var coefficient = coefficientList.Where(x=>x.TypeDate == typeDate && x.IsDeleted == false).FirstOrDefault();


                                var entity = new AnnualWorkingDay
                                {
                                    Day = day,
                                    ShiftType = shiftType,
                                    TypeDate = typeDate,
                                    CoefficientId = coefficient.Id
                                };

                                annualWorkingDays.Add(entity);
                            }
                        }
                        else if (dayCell != null && typeCell != null)
                        {
                            DateTime day;
                            if (DateTime.TryParse(dayCell.ToString(), out day))
                            {
                                if (_context.AnnualWorkingDays.Any(x => x.Day == day))
                                {
                                    continue; // Bỏ qua hàng này và chuyển sang hàng tiếp theo
                                }
                                var coefficient = coefficientList.Where(x => x.TypeDate == TypeDate.Holiday && x.IsDeleted == false).FirstOrDefault();


                                var entity = new AnnualWorkingDay
                                {
                                    Day = day,
                                    ShiftType = configDay.Holiday,
                                    TypeDate = TypeDate.Holiday,
                                    CoefficientId = coefficient.Id
                                };

                                annualWorkingDays.Add(entity);
                            }
                        }
                    }


                    if (annualWorkingDays.Count == 0)
                    {
                        throw new Exception("Không tìm thấy ngày làm việc hàng năm hợp lệ trong tệp Excel.");
                    }
                    _context.AnnualWorkingDays.AddRange(annualWorkingDays);
                    await _context.SaveChangesAsync(cancellationToken);

                    return annualWorkingDays.FirstOrDefault()?.Id.ToString() ?? Guid.Empty.ToString();
                }
            }
           
        }
        catch (InvalidDataException ex)
        {
            throw new Exception("Dữ liệu trong file không đúng định dạng!");
        }
        catch (IOException ex)
        {
            throw new Exception("Đã xảy ra lỗi khi truy cập vào file!");
        }


    }
}

