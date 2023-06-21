using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;

namespace mentor_v1.Application.MaternityEmployee.Queries;
public class GetMaternityEmployeeRequest : IRequest<PaginatedList<GetMaternityEmployeeViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetMaternityEmployeeRequestHandler : IRequestHandler<GetMaternityEmployeeRequest, PaginatedList<GetMaternityEmployeeViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMaternityEmployeeRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public Task<PaginatedList<GetMaternityEmployeeViewModel>> Handle(GetMaternityEmployeeRequest request, CancellationToken cancellationToken)
    {
        var matern = _context.Get<Domain.Entities.MaternityEmployee>().Where(x => x.IsDeleted == false).AsNoTracking();
        var model = _mapper.ProjectTo<GetMaternityEmployeeViewModel>(matern);
        var page = PaginatedList<GetMaternityEmployeeViewModel>.CreateAsync(model, request.Page, request.Size);
        return page;
    }
}
