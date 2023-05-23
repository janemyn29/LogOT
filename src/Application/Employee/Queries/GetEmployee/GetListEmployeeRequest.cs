using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Employee.Queries.GetEmployee;
public class GetListEmployeeRequest : IRequest<PaginatedList<EmployeeViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

// IRequestHandler<request type, return type>
public class GetListEmployeeRequestHandler : IRequestHandler<GetListEmployeeRequest, PaginatedList<EmployeeViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListEmployeeRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    async Task<PaginatedList<EmployeeViewModel>> IRequestHandler<GetListEmployeeRequest, PaginatedList<EmployeeViewModel>>.Handle(GetListEmployeeRequest request, CancellationToken cancellationToken)
    {
        // get list employee
        var ListEmployee = _context.Get<Domain.Entities.Employee>()
            .Include(x => x.ApplicationUser)
            .Where(x => x.IsDeleted == false).AsNoTracking();
        var map = _mapper.ProjectTo<EmployeeViewModel>(ListEmployee);

        // Paginate data
        var page = await PaginatedList<EmployeeViewModel>
            .CreateAsync(map, request.Page, request.Size);
        return page;
    }
}