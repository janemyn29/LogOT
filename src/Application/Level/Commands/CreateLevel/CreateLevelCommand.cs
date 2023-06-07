﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Entities;

namespace mentor_v1.Application.Level.Commands.CreateLevel;

public class CreateLevelCommand : IRequest<Guid>
{
    public string Name { get; set; }

    public string Description { get; set; }

    public IList<Position> Positions { get; set; }

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
        // create new Level from request data
        var Level = new Domain.Entities.Level()
        {
            Name = request.Name,
            Description = request.Description,
            Positions = request.Positions
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