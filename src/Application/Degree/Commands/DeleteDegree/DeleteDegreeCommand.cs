using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Degree.Commands.DeleteDegree;
public class DeleteDegreeCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteDegreeCommandshandler : IRequestHandler<DeleteDegreeCommand, bool>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public DeleteDegreeCommandshandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<bool> Handle(DeleteDegreeCommand request, CancellationToken cancellationToken)
    {
        var findDegree = await _applicationDbContext.Get<Domain.Entities.Degree>().FindAsync(new object[] { request.Id}, cancellationToken);
        
        if (findDegree == null || findDegree.IsDeleted == true) {
            throw new NotFoundException("Không tìm thấy ID " + request.Id + ". Xoá thất bại.");
        }

        findDegree.IsDeleted = true;
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return true;

    }
}