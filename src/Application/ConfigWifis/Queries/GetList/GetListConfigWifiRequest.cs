
using System.Data.Entity;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Allowance.Queries.GetAllowance;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;

namespace mentor_v1.Application.ConfigWifis.Queries.GetList;
public class GetListConfigWifiRequest : IRequest<PaginatedList<ConfigWifiViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetListConfigWifiRequestHandler : IRequestHandler<GetListConfigWifiRequest, PaginatedList<ConfigWifiViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetListConfigWifiRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public Task<PaginatedList<ConfigWifiViewModel>> Handle(GetListConfigWifiRequest request, CancellationToken cancellationToken)
    {
        var entity = _context.Get<Domain.Entities.ConfigWifi>().Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<ConfigWifiViewModel>(entity);

        var page = PaginatedList<ConfigWifiViewModel>.CreateAsync(map, request.Page, request.Size);
        return page;
    }
}
