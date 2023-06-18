using System.Data.Entity;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Dependent.Queries;
public class GetDependentIdRequest : IRequest<GetDependentViewModel>
{
    public Guid id { get; set; }
}

public class GetDependentRequestIdHandler : IRequestHandler<GetDependentIdRequest, GetDependentViewModel>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    public GetDependentRequestIdHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<GetDependentViewModel> Handle(GetDependentIdRequest request, CancellationToken cancellationToken)
    {
        var dependent = _applicationDbContext.Get<Domain.Entities.Dependent>().Where(x => x.Id.Equals(request.id) && x.IsDeleted == false).AsNoTracking().FirstOrDefault();
        if (dependent == null) throw new NotFoundException("Không tìm thấy ID " + request.id);
        var map = _mapper.Map<GetDependentViewModel>(dependent);
        return Task.FromResult(map);

    }
}
