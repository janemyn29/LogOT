using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Department.Queries.GetDepartmentWithRelativeObjet;

public class GetDepartmentByIdRequest : IRequest<GetDepartment.DepartmentViewModel>
{
    public Guid Id { get; set; }

}

// IRequestHandler<request type, return type>
public class GetDepartmentByIdRequestHandler : IRequestHandler<GetDepartmentByIdRequest, GetDepartment.DepartmentViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetDepartmentByIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<GetDepartment.DepartmentViewModel> Handle(GetDepartmentByIdRequest request, CancellationToken cancellationToken)
    {
        var Department = _context.Get<Domain.Entities.Department>()
            .Include(x => x.Positions)
            .Where(x => x.IsDeleted == false && x.Id.Equals(request.Id))
            .AsNoTracking().FirstOrDefault();
        if (Department == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Department), request.Id);
        }

        // AsNoTracking to remove default tracking on entity framework
        var map = _mapper.Map<GetDepartment.DepartmentViewModel>(Department);

        // Paginate data
        return Task.FromResult(map); //Task.CompletedTask;
    }
}