using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Allowance.Queries;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Allowance.Commands.UpdateAllowance;
public class UpdateAllowanceCommand : IRequest
{
    public UpdateAllowanceViewModel updateAllowanceView;
}

public class UpdateAllowanceCommandHandler : IRequestHandler<UpdateAllowanceCommand>
{ 
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public UpdateAllowanceCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateAllowanceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Get<Domain.Entities.Allowance>()
                    .FindAsync(new object[] { request.updateAllowanceView.Id }, cancellationToken);
        if (entity == null || entity.IsDeleted == true)
        {
            throw new NotFoundException("Không tìm thấy ID " + request.updateAllowanceView.Id);
        }

        var mapEnity = _mapper.Map(request.updateAllowanceView, entity);

        if (await _context.SaveChangesAsync(cancellationToken) == 0)
            throw new Exception();
            
        return Unit.Value;
    }
}