﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.DepartmentAllowance.Queries.GetDepartmentAllowanceWithRelativeObject;

public class GetDepartmentAllowanceByDepartmentIdRequest : IRequest<GetDepartmentAllowance.DepartmentAllowanceViewModel>
{
    public Guid Id { get; set; }

}

// IRequestHandler<request type, return type>
public class GetDepartmentAllowanceByDepartmentIdRequestHandler : IRequestHandler<GetDepartmentAllowanceByDepartmentIdRequest, GetDepartmentAllowance.DepartmentAllowanceViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetDepartmentAllowanceByDepartmentIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<GetDepartmentAllowance.DepartmentAllowanceViewModel> Handle(GetDepartmentAllowanceByDepartmentIdRequest request, CancellationToken cancellationToken)
    {
        var DepartmentAllowance = _context.Get<Domain.Entities.DepartmentAllowance>()
            .Where(x => x.IsDeleted == false && x.DepartmentId.Equals(request.Id))
            .AsNoTracking().FirstOrDefault();
        if (DepartmentAllowance == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.DepartmentAllowance), request.Id);
        }

        // AsNoTracking to remove default tracking on entity framework
        var map = _mapper.Map<GetDepartmentAllowance.DepartmentAllowanceViewModel>(DepartmentAllowance);

        // Paginate data
        return Task.FromResult(map); //Task.CompletedTask;
    }
}
