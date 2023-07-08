using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Note.Queries.GetNotification;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Note.Queries.GetNotificationByRelativeObject;

public class GetListNotificationByUserIdRequest : IRequest<PaginatedList<NotificationViewModel>>
{
    public string userId { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetListNotificationByUserIdRequestHandler : IRequestHandler<GetListNotificationByUserIdRequest, PaginatedList<NotificationViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListNotificationByUserIdRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<NotificationViewModel>> Handle(GetListNotificationByUserIdRequest request, CancellationToken cancellationToken)
    {
        //get Notification 
        var Notifications = _applicationDbContext.Get<Domain.Entities.Notification>()
            .Include(x => x.ApplicationUser)
            .Where(x => x.IsDeleted == false && x.ApplicationUserId.Equals(request.userId))
            .OrderByDescending(x => x.Created)
            .AsNoTracking();

        var models = _mapper.ProjectTo<NotificationViewModel>(Notifications);

        var page = PaginatedList<NotificationViewModel>.CreateAsync(models, request.Page, request.Size);

        return page;
    }
}
