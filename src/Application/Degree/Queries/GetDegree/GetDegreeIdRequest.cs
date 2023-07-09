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

namespace mentor_v1.Application.Degree.Queries.GetDegree;
public class GetDegreeIdRequest : IRequest<GetDegreeViewModel>
{
    public Guid id { get; set; }
}

public class GetDegreeRequestIdHandler : IRequestHandler<GetDegreeIdRequest, GetDegreeViewModel>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    public GetDegreeRequestIdHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<GetDegreeViewModel> Handle(GetDegreeIdRequest request, CancellationToken cancellationToken)
    {
        var degree = _applicationDbContext.Get<Domain.Entities.Degree>().Where(x => x.Id.Equals(request.id) && x.IsDeleted == false).AsNoTracking().FirstOrDefault();
        if (degree == null) throw new NotFoundException("Không tìm thấy ID " + request.id);
        var map = _mapper.Map<GetDegreeViewModel>(degree);
        return Task.FromResult(map);
        
    }
}
