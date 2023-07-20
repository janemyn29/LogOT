using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.MaternityEmployee.Queries;
public class GetMaternityEmployeeIdRequest : IRequest<GetMaternityEmployeeViewModel>
{
    public Guid Id { get; set; }
}
public class GetMaternityEmployeeIdRequestHandler : IRequestHandler<GetMaternityEmployeeIdRequest, GetMaternityEmployeeViewModel>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetMaternityEmployeeIdRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<GetMaternityEmployeeViewModel> Handle(GetMaternityEmployeeIdRequest request, CancellationToken cancellationToken)
    {
        var maternity = _applicationDbContext.Get<Domain.Entities.MaternityEmployee>().Include(x=>x.ApplicationUser).Where(x => x.Id.Equals(request.Id) && x.IsDeleted == false).AsNoTracking().FirstOrDefault();
        if (maternity == null) throw new NotFoundException("Không tìm thấy ID: " + request.Id);
        var map = _mapper.Map<GetMaternityEmployeeViewModel>(maternity);
        return Task.FromResult(map);
    }
}
