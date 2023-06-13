using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;

namespace mentor_v1.Application.Positions.Commands.CreatePosition;
public class CreatePositionCommand : IRequest<Guid>
{
    public Guid DepartmentId { get; set; }
    public Guid LevelId { get; set; }
    public string Name { get; set; }
}

public class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreatePositionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
    {
        var position = new Domain.Entities.Position()
        {
          DepartmentId = request.DepartmentId,
          LevelId = request.LevelId,
          Name = request.Name,
        };

        // add new category
        _context.Get<Domain.Entities.Position>().Add(position);

        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);

        // return the Guid
        return position.Id;
    }
}

