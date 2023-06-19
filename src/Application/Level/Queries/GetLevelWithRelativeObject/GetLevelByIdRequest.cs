using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Level.Queries.GetLevelWithRelativeObject;

public class GetLevelByIdRequest : IRequest<Domain.Entities.Level>
{
    public Guid Id { get; set; }

}

// IRequestHandler<request type, return type>
public class GetLevelByIdRequestHandler : IRequestHandler<GetLevelByIdRequest, Domain.Entities.Level>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetLevelByIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.Level> Handle(GetLevelByIdRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var Level = _context.Get<Domain.Entities.Level>()
            .Include(x => x.Positions)
            .Where(x => x.IsDeleted == false && x.Id.Equals(request.Id))
            .AsNoTracking().FirstOrDefault();
        if (Level == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Level), request.Id);
        }

        // AsNoTracking to remove default tracking on entity framework
        //var map = _mapper.Map<GetLevel.LevelViewModel>(Level);

        // Paginate data
        return Task.FromResult(Level); //Task.CompletedTask;
    }
}