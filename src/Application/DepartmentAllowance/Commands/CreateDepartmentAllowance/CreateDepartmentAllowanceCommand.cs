using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.DepartmentAllowance.Commands.CreateDepartmentAllowance;

public class CreateDepartmentAllowanceCommand : IRequest<Guid>
{
    public Guid DepartmentId { get; set; }
    public Guid SubsidizeId { get; set; }

}

public class CreateDepartmentAllowanceCommandHandler : IRequestHandler<CreateDepartmentAllowanceCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateDepartmentAllowanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDepartmentAllowanceCommand request, CancellationToken cancellationToken)
    {

        var DepartmentAllowance = new Domain.Entities.DepartmentAllowance()
        {
            DepartmentId = request.DepartmentId,
            SubsidizeId = request.SubsidizeId,

        };

        _context.Get<Domain.Entities.DepartmentAllowance>().Add(DepartmentAllowance);

        await _context.SaveChangesAsync(cancellationToken);

        return DepartmentAllowance.Id;
    }
}