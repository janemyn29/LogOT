using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Subsidize.Commands.CreateSubsidize;

public class CreateSubsidizeCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Amount { get; set; }

}

public class CreateSubsidizeCommandHandler : IRequestHandler<CreateSubsidizeCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateSubsidizeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateSubsidizeCommand request, CancellationToken cancellationToken)
    {

        var Subsidize = new Domain.Entities.Subsidize()
        {
            Name = request.Name,
            Description = request.Description,
            Amount = request.Amount,

        };

        _context.Get<Domain.Entities.Subsidize>().Add(Subsidize);

        await _context.SaveChangesAsync(cancellationToken);

        return Subsidize.Id;
    }
}