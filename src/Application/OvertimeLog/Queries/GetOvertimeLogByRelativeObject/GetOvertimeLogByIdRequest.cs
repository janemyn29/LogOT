using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLogByRelativeObject;

public class GetOvertimeLogByIdRequest : IRequest<Domain.Entities.OvertimeLog>
{
    public Guid Id { get; set; }
    public Domain.Identity.ApplicationUser user { get; set; }
    public string Role { get; set; }

}

// IRequestHandler<request type, return type>
public class GetOvertimeLogByIdRequestHandler : IRequestHandler<GetOvertimeLogByIdRequest, Domain.Entities.OvertimeLog>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetOvertimeLogByIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.OvertimeLog> Handle(GetOvertimeLogByIdRequest request, CancellationToken cancellationToken)
    {
        // get categories
        Domain.Entities.OvertimeLog OvertimeLog = null;
        if (request.Role.ToLower().Equals("manager"))
        {
            OvertimeLog = _context.Get<Domain.Entities.OvertimeLog>()
                .Include(x => x.ApplicationUser)
           .Where(x => x.IsDeleted == false && x.Id.Equals(request.Id))
           .AsNoTracking().FirstOrDefault();
        }
        else if (request.Role.ToLower().Equals("employee"))
        {
            OvertimeLog = _context.Get<Domain.Entities.OvertimeLog>()
                .Include(x => x.ApplicationUser)
            .Where(x => x.IsDeleted == false && x.Id.Equals(request.Id) && x.ApplicationUserId.Equals(request.user.Id))
            .AsNoTracking().FirstOrDefault();
        }

        if (OvertimeLog == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.OvertimeLog), request.Id);
        }

        // AsNoTracking to remove default tracking on entity framework
        //var map = _mapper.Map<GetOvertimeLog.OvertimeLogViewModel>(OvertimeLog);

        // Paginate data
        return Task.FromResult(OvertimeLog); //Task.CompletedTask;
    }
}