
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Allowance.Commands.DeleteAllowance;
public class DeleteAllowanceCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteAllowanceCommandHandler : IRequestHandler<DeleteAllowanceCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteAllowanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteAllowanceCommand request, CancellationToken cancellationToken)
    {

        var allowanceEmployee = await _context.Get<Domain.Entities.AllowanceEmployee>().Where(x => x.AllowanceId.Equals(request.Id) && x.IsDeleted == false).FirstOrDefaultAsync();

        if (allowanceEmployee != null)
        {
            throw new Exception();
        }

        var currentAllowance = await _context.Get<Domain.Entities.Allowance>().FindAsync(new object[] { request.Id }, cancellationToken);

        if (currentAllowance == null || currentAllowance.IsDeleted == true)
        {
            throw new NotFoundException("Không tìm thấy ID " + request.Id + ". Xoá thất bại.");
        }
        currentAllowance.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
