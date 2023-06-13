using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Positions.Commands.DeletePosition;
public record DeletePositionCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}

public class DeletePositionCommandHandler : IRequestHandler<DeletePositionCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeletePositionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
    {
        
        var city = _context.Get<Domain.Entities.Position>().Include(x => x.ApplicationUsers).Where(x => x.IsDeleted == false && x.Id.Equals(request.Id)).FirstOrDefault();

        if (city == null)
        {
            throw new NotFoundException("Không tìm thấy vị trí công việc mà bạn yêu cầu!"); ;
        }
        if (city.ApplicationUsers != null)
        {
            throw new NotFoundException("Không thể xóa vị trí công việc này vì hiện vẫn có nhân viên làm vị trí này!");
        }
        else
        {
            city.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}
