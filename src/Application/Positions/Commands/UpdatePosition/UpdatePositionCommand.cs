using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.Positions.Commands.UpdatePosition;
public record UpdatePositionCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid LevelId { get; set; }
    public string Name { get; set; }
}

public class UpdatePositionCommandHandler : IRequestHandler<UpdatePositionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdatePositionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
    {
        var position = _context.Get<Domain.Entities.Position>().Where(x=>x.Id == request.Id && x.IsDeleted == false).FirstOrDefault();

        if (position == null)
        {
            throw new NotFoundException("Không tìm thấy vị trí công việc mà bạn yêu cầu!");
        }
        position.DepartmentId = request.DepartmentId;
        position.LevelId = request.LevelId;
        position.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

