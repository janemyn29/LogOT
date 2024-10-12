using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Subsidize.Queries.GetSubsidize;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Subsidize.Queries.GetSubsidizeWithRelativeObject;
public class GetListSubsidizeByDepartmentId : IRequest<PaginatedList<SubsidizeViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public Guid departmentId { get; set; }
}

public class GetListSubsidizeByDepartmentIdHandler : IRequestHandler<GetListSubsidizeByDepartmentId, PaginatedList<SubsidizeViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListSubsidizeByDepartmentIdHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<SubsidizeViewModel>> Handle(GetListSubsidizeByDepartmentId request, CancellationToken cancellationToken)
    {

        //get Subsidize by ?
        var Subsidizes = _applicationDbContext.Get<Domain.Entities.Subsidize>().Include(x=>x.DepartmentAllowances).Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<SubsidizeViewModel>(Subsidizes);

        var page = PaginatedList<SubsidizeViewModel>.CreateAsync(models, request.Page, request.Size);

        return page;
    }
}