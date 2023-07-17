using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Files;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Common.PagingUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.ApplicationUser.Queries.GetUser;
public class GetListUserRequest : IRequest<PagingAppUser<Domain.Identity.ApplicationUser>>
{
    public int Page { get; set; }
    public int Size { get; set; }

}

// IRequestHandler<request type, return type>
public class GetListUserRequestHandler : IRequestHandler<GetListUserRequest, PagingAppUser<Domain.Identity.ApplicationUser>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManage;

    // DI
    public GetListUserRequestHandler(IApplicationDbContext context, IMapper mapper, UserManager<Domain.Identity.ApplicationUser> userManage)
    {
        _context = context;
        _mapper = mapper;
        _userManage = userManage;
    }

    public async Task<PagingAppUser<Domain.Identity.ApplicationUser>> Handle(GetListUserRequest request, CancellationToken cancellationToken)
    {
        var ListApplicationUser = await _userManage.GetUsersInRoleAsync("Employee");
        var list = ListApplicationUser.OrderBy(x => x.Fullname).ToList();

        // Paginate data
        var page = await PagingAppUser<Domain.Identity.ApplicationUser>
            .CreateAsync(list, request.Page, request.Size);
/*        foreach (var item in page.Items)
        {
            if (item.Image != null)
            {
                item.ImageBase = _file.CovertToBase64(item.Image);
            }
        }*/
        return page;
    }
}