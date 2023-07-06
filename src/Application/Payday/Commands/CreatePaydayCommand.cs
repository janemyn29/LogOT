using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.Payday.Commands;
public class CreatePaydayCommand : IRequest<Guid>
{
    public DateTime PaymentDay { get; set; }
}

public class CreatePaydayCommandHandler : IRequestHandler<CreatePaydayCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;


    public CreatePaydayCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Guid> Handle(CreatePaydayCommand request, CancellationToken cancellationToken)
    {

        var item = new Domain.Entities.PayDay()
        {
            PaymentDay = request.PaymentDay,
        };
        // add new category
        _context.Get<Domain.Entities.PayDay>().Add(item);

        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);

        return item.Id;
    }
}

