using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.DetailTax;
public class CreateDetailTaxCommand : IRequest<Guid>
{
    public Domain.Entities.DetailTax DetailTax { get; set; }
    
}

public class CreateDetailTaxCommandHandler : IRequestHandler<CreateDetailTaxCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;


    public CreateDetailTaxCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Guid> Handle(CreateDetailTaxCommand request, CancellationToken cancellationToken)
    {
        // add new category
        _context.Get<Domain.Entities.DetailTax>().Add(request.DetailTax);

        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);

        return request.DetailTax.Id;
    }
}
