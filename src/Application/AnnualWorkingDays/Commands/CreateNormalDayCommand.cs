using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.AnnualWorkingDays.Commands;
public class CreateNormalDayCommand : IRequest<Guid>
{
    public DateTime Day { get; set; }
}

public class CreateNormalDayCommandHandler : IRequestHandler<CreateNormalDayCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;


    public CreateNormalDayCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Guid> Handle(CreateNormalDayCommand request, CancellationToken cancellationToken)
    {
        TypeDate typeDate;
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
        else
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

    }
}

