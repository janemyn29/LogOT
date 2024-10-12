using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Note.Queries.GetNotification;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Note.Queries.GetNotificationByRelativeObject;
public class GetNotificationById : IRequest<NotificationViewModel>
{
    public Guid Id { get; set; }
}

public class GetNotificationByIdHandler : IRequestHandler<GetNotificationById,NotificationViewModel>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetNotificationByIdHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<NotificationViewModel> Handle(GetNotificationById request, CancellationToken cancellationToken)
    {
        //get Notification 
        var Notifications = _applicationDbContext.Get<Domain.Entities.Notification>()
            .Include(x => x.ApplicationUser)
            .Where(x => x.IsDeleted == false && x.Id == request.Id ).FirstOrDefault();

        if(Notifications  == null) {
            throw new Exception("Không tìm thấy thông báo bạn yêu cầu!");
        }
        var models = _mapper.Map<NotificationViewModel>(Notifications);

        Notifications.IsRead = true;
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return models;
    }
}

