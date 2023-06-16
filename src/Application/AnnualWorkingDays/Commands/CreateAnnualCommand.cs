using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.AnnualWorkingDays.Commands;
public class CreateAnnualCommand : IRequest<Guid>
{
    public DateTime Day { get; set; }
    public bool IsHoliday { get; set; }
}

public class CreateAnnualCommandHandler : IRequestHandler<CreateAnnualCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;


    public CreateAnnualCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Guid> Handle(CreateAnnualCommand request, CancellationToken cancellationToken)
    {
       var existedDay = _context.AnnualWorkingDays.Where(x=>x.IsDeleted==false && x.Day.Date == request.Day.Date).FirstOrDefault();
        if(existedDay != null)
        {
            throw new InvalidDataException("Ngày " + request.Day.ToString("dd/MM/yyyy") + " đã tồn tại!");
        }else if(DateTime.Now.Date > request.Day.Date)
        {
            throw new InvalidDataException("Ngày được thêm vào phải lớn hơn hoặc nhỏ hơn ngày hiện tại! ");

        }
        try
        {
            TypeDate typeDate ;
            ShiftType shiftType;
            Guid coeId;
            if (request.Day.DayOfWeek == DayOfWeek.Saturday)
            {
                shiftType = _context.ConfigDays.FirstOrDefault().Saturday;
                typeDate = TypeDate.Saturday;
                coeId = _context.Coefficients.Where(x => x.TypeDate == typeDate).FirstOrDefault().Id;
                var city = new Domain.Entities.AnnualWorkingDay()
                {
                    Day = request.Day,
                    CoefficientId = coeId,
                    TypeDate = typeDate,
                    ShiftType = shiftType,

                };
                // add new category
                _context.Get<Domain.Entities.AnnualWorkingDay>().Add(city);

                // commit change to database
                // because the function is async so we await it
                await _context.SaveChangesAsync(cancellationToken);

                // return the Guid
                return city.Id;
            }
            else if (request.Day.DayOfWeek == DayOfWeek.Sunday)
            {
                shiftType = _context.ConfigDays.FirstOrDefault().Sunday;
                typeDate = TypeDate.Sunday;
                coeId = _context.Coefficients.Where(x => x.TypeDate == typeDate).FirstOrDefault().Id;
                

                var city = new Domain.Entities.AnnualWorkingDay()
                {
                    Day = request.Day,
                    CoefficientId = coeId,
                    TypeDate = typeDate,
                    ShiftType = shiftType,

                };
                // add new category
                _context.Get<Domain.Entities.AnnualWorkingDay>().Add(city);

                // commit change to database
                // because the function is async so we await it
                await _context.SaveChangesAsync(cancellationToken);

                // return the Guid
                return city.Id;
            }
            else if (request.IsHoliday)
            {

                shiftType = _context.ConfigDays.FirstOrDefault().Holiday;
                typeDate = TypeDate.Holiday;
                coeId = _context.Coefficients.Where(x => x.TypeDate == typeDate).FirstOrDefault().Id;
                var city = new Domain.Entities.AnnualWorkingDay()
                {
                    Day = request.Day,
                    CoefficientId = coeId,
                    TypeDate = typeDate,
                    ShiftType = shiftType,

                };
                // add new category
                _context.Get<Domain.Entities.AnnualWorkingDay>().Add(city);

                // commit change to database
                // because the function is async so we await it
                await _context.SaveChangesAsync(cancellationToken);

                // return the Guid
                return city.Id;
            }else
            {

                shiftType = _context.ConfigDays.FirstOrDefault().Normal;
                typeDate = TypeDate.Normal;
                coeId = _context.Coefficients.Where(x => x.TypeDate == typeDate).FirstOrDefault().Id;
                var city = new Domain.Entities.AnnualWorkingDay()
                {
                    Day = request.Day,
                    CoefficientId = coeId,
                    TypeDate = typeDate,
                    ShiftType = shiftType,

                };
                // add new category
                _context.Get<Domain.Entities.AnnualWorkingDay>().Add(city);

                // commit change to database
                // because the function is async so we await it
                await _context.SaveChangesAsync(cancellationToken);

                // return the Guid
                return city.Id;
            }
            throw new Exception("Đã xảy ra lỗi trong quá trình khởi tạo!");


        }
        catch (Exception)
        {

            throw new Exception("Đã xảy ra lỗi trong quá trình khởi tạo!");
        }
        
       
    }
}
