using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.ApplicationUser.Queries.GetUser;
public class GetUserByIdRequest : IRequest<Domain.Identity.ApplicationUser>
{
    public string Id { get; set; }

}

// IRequestHandler<request type, return type>
public class GetUserByIdRequestHandler : IRequestHandler<GetUserByIdRequest, Domain.Identity.ApplicationUser>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;

    // DI
    public GetUserByIdRequestHandler(IApplicationDbContext context, IMapper mapper, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    public Task<Domain.Identity.ApplicationUser> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var city = _userManager.Users.Where(x => x.WorkStatus==Domain.Enums.WorkStatus.StillWork && x.Id.Equals(request.Id)).AsNoTracking().FirstOrDefault();
        if (city == null)
        {
            throw new NotFoundException("Không tìm thấy id!");
        }
        // map IQueryable<BlogCity> to IQueryable<GetCity.CityViewModel>
        // AsNoTracking to remove default tracking on entity framework

        // Paginate data
        return Task.FromResult(city); //Task.CompletedTask;
    }
}


