using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.MaternityEmployee.Queries;
public class GetMaternityEmployeeRequest : IRequest<PaginatedList<Domain.Entities.MaternityEmployee>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetMaternityEmployeeRequestHandler : IRequestHandler<GetMaternityEmployeeRequest, PaginatedList<Domain.Entities.MaternityEmployee>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMaternityEmployeeRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public Task<PaginatedList<Domain.Entities.MaternityEmployee>> Handle(GetMaternityEmployeeRequest request, CancellationToken cancellationToken)
    {
        var matern = _context.Get<Domain.Entities.MaternityEmployee>().Include(x => x.ApplicationUser).Where(x => x.IsDeleted == false).AsNoTracking();

        //var model = _mapper.ProjectTo<Domain.Entities.MaternityEmployee>(matern);

        var page = PaginatedList<Domain.Entities.MaternityEmployee>.CreateAsync(matern, request.Page, request.Size);

        return page;
    }
}
