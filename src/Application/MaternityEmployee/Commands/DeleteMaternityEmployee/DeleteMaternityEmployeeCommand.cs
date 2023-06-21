using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.MaternityEmployee.Commands.DeleteMaternityEmployee;
public class DeleteMaternityEmployeeCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteMaternityEmployeeCommandHandle : IRequestHandler<DeleteMaternityEmployeeCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteMaternityEmployeeCommandHandle(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteMaternityEmployeeCommand request, CancellationToken cancellationToken)
    {
        var findMaternity = await _context.Get<Domain.Entities.MaternityEmployee>().FindAsync(new object[] { request.Id }, cancellationToken);

        if (findMaternity == null || findMaternity.IsDeleted == true)
        {
            throw new NotFoundException("Không tìm thấy ID " + request.Id + ". Xoá thất bại.");
        }

        findMaternity.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
