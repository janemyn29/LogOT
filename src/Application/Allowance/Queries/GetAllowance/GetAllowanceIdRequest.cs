using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Allowance.Queries.GetAllowance;
public class GetAllowanceIdRequest : IRequest<AllowanceViewModel>
{
    public Guid Id { get; set; }
}

public class GetAllowanceIdRequestHandler : IRequestHandler<GetAllowanceIdRequest, AllowanceViewModel>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetAllowanceIdRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<AllowanceViewModel> Handle(GetAllowanceIdRequest request, CancellationToken cancellationToken)
    {
        var allowance = _applicationDbContext.Get<Domain.Entities.Allowance>().Where(x => x.Id.Equals(request.Id) && x.IsDeleted == false).AsNoTracking().FirstOrDefault();
        if (allowance == null)
        {
            throw new NotFoundException("Không tìm thấy ID " + request.Id);
        }
        var map = _mapper.Map<AllowanceViewModel>(allowance);
        return Task.FromResult(map);
    }
}