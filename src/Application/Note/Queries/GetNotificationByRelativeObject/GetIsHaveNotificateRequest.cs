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
public class GetIsHaveNotificateRequest : IRequest<NoteModel>
{
    public string userId { get; set; }
}

public class GetIsHaveNotificateRequestHandler : IRequestHandler<GetIsHaveNotificateRequest, NoteModel>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetIsHaveNotificateRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<NoteModel> Handle(GetIsHaveNotificateRequest request, CancellationToken cancellationToken)
    {
        //get Notification 
        var Notifications = _applicationDbContext.Get<Domain.Entities.Notification>()
            .Include(x => x.ApplicationUser)
            .Where(x => x.IsDeleted == false && x.ApplicationUserId.Equals(request.userId) && x.IsRead == false)
            .OrderByDescending(x => x.Created)
            .AsNoTracking().ToList();
        NoteModel noteModel = new NoteModel();
        if (Notifications != null && Notifications.Count > 0 )
        {
           
            noteModel.Number = Notifications.Count;
            noteModel.IsHaveNoti = true;
            return Task.FromResult(noteModel);
        }
         
            noteModel.Number = Notifications.Count;
            noteModel.IsHaveNoti = false;
        return Task.FromResult(noteModel);

    }
}

