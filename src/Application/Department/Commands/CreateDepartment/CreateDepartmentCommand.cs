using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Entities;

namespace mentor_v1.Application.Department.Commands.CreateDepartment;
public class CreateDepartmentCommand : IRequest<Guid>
{
    public string Name { get; set; }

    public string Description { get; set; }

}

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {

        var Department = new Domain.Entities.Department()
        {
            Name = request.Name,
            Description = request.Description,
           
        };

        _context.Get<Domain.Entities.Department>().Add(Department);

        await _context.SaveChangesAsync(cancellationToken);

        return Department.Id;
    }
}
