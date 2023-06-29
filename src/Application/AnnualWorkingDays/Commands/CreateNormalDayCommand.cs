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
        var shiftType = _context.ConfigDays.FirstOrDefault().Normal;
        var typeDate = TypeDate.Normal;
        var coeId = _context.Coefficients.Where(x => x.TypeDate == typeDate).FirstOrDefault().Id;
        var city = new AnnualWorkingDay()
        {
            Day = request.Day.Date,
            CoefficientId = coeId,
            TypeDate = typeDate,
            ShiftType = shiftType,

        };
        // add new category
        _context.Get<AnnualWorkingDay>().Add(city);

        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);

        // return the Guid
        return city.Id;
    }
}

