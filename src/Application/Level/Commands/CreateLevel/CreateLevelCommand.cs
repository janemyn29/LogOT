using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Level.Queries.GetLevel;
using mentor_v1.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace mentor_v1.Application.Level.Commands.CreateLevel;

public class CreateLevelCommand : IRequest<Guid>
{
    public LevelViewModel levelViewModel { get; set; }

}

// Handler to handle the request (Can be written to another file)
// CreateLevelCommand : IRequest<Guid> => IRequestHandler<CreateLevelCommand, Guid>
public class CreateLevelCommandHandler : IRequestHandler<CreateLevelCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateLevelCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateLevelCommand request, CancellationToken cancellationToken)
    {
        var tempLevel = _context.Get<Domain.Entities.Level>()
            .Where(l => l.Name.Equals(request.levelViewModel.Name)).FirstOrDefault();

        if (tempLevel != null) 
        {
            throw new Exception("Tên cấp độ đã tồn tại");
        }

        // create new Level from request data
        var Level = new Domain.Entities.Level()
        {
            Name = request.levelViewModel.Name,
            Description = request.levelViewModel.Description,
            //Positions = request.levelViewModel.Positions
        };

        // add new Level
        _context.Get<Domain.Entities.Level>().Add(Level);

        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);

        // return the Guid
        return Level.Id;
    }
}