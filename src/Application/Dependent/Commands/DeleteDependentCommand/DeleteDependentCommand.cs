using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Dependent.Commands.DeleteDependentCommand;
public class DeleteDependentCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteDependentCommandshandler : IRequestHandler<DeleteDependentCommand, bool>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public DeleteDependentCommandshandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<bool> Handle(DeleteDependentCommand request, CancellationToken cancellationToken)
    {
        var findDegree = await _applicationDbContext.Get<Domain.Entities.Dependent>().FindAsync(new object[] { request.Id }, cancellationToken);

        if (findDegree == null || findDegree.IsDeleted == true)
        {
            throw new NotFoundException("Không tìm thấy ID " + request.Id + ". Xoá thất bại.");
        }

        findDegree.IsDeleted = true;
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return true;

    }
}