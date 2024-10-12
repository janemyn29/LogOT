using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.Note.Commands;

public class CreateNotiForRequestsCommand : IRequest<Guid>
{
    public string ApplicationUserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
}

public class CreateNotiWithTransactionCommandHandler : IRequestHandler<CreateNotiForRequestsCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;


    public CreateNotiWithTransactionCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Guid> Handle(CreateNotiForRequestsCommand request, CancellationToken cancellationToken)
    {
        Notification job; 
        try
        {
            job = new Domain.Entities.Notification()
            {
                Title = request.Title,
                Description = request.Description,
                ApplicationUserId = request.ApplicationUserId,
                IsRead = false
            };
            // add new category
            _context.Get<Domain.Entities.Notification>().Add(job);
        }
        catch (Exception)
        {

            throw new Exception("Khởi tạo thông báo không thành công ");
        }
        
        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}