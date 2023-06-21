using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.SkillEmployee.Commands.DeleteSkillEmployeeCommand;
public class DeleteSkillEmployeeCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteSkillEmployeeCommandHandler : IRequestHandler<DeleteSkillEmployeeCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteSkillEmployeeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> Handle(DeleteSkillEmployeeCommand request, CancellationToken cancellationToken)
    {
        var findskill = await _context.Get<Domain.Entities.SkillEmployee>().FindAsync(new object[] { request.Id, cancellationToken });
       
        if (findskill == null) throw new NotFoundException("Không tìm thấy ID " + request.Id + ". Xoá thất bại.");

        findskill.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}