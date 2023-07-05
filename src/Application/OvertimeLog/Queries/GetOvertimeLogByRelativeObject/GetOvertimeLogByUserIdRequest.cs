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
using mentor_v1.Application.Common.Models;

namespace mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLogByRelativeObject;

public class GetOvertimeLogByUserIdRequest : IRequest<PaginatedList<Domain.Entities.OvertimeLog>>
{
    public Guid id { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetOvertimeLogByUserIdRequestHandler : IRequestHandler<GetOvertimeLogByUserIdRequest, PaginatedList<Domain.Entities.OvertimeLog>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetOvertimeLogByUserIdRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<Domain.Entities.OvertimeLog>> Handle(GetOvertimeLogByUserIdRequest request, CancellationToken cancellationToken)
    {

        //get OvertimeLog 
        var OvertimeLogs = _applicationDbContext.Get<Domain.Entities.OvertimeLog>().Where(x => x.IsDeleted == false && x.ApplicationUserId.Equals(request.id)).OrderByDescending(x => x.Created).AsNoTracking();
        //var models = _mapper.ProjectTo<OvertimeLogViewModel>(OvertimeLogs);

        var page = PaginatedList<Domain.Entities.OvertimeLog>.CreateAsync(OvertimeLogs, request.Page, request.Size);

        return page;
    }
}