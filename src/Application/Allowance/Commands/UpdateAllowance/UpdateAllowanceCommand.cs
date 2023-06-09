using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Allowance.Queries;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Allowance.Commands.UpdateAllowance;
public class UpdateAllowanceCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int AllowanceType { get; set; }
    public float Amount { get; set; }
    public string Eligibility_Criteria { get; set; }
    public string Requirements { get; set; }
}

public class UpdateAllowanceCommandHandler : IRequestHandler<UpdateAllowanceCommand>
{ 
    private readonly IApplicationDbContext _context;
    public UpdateAllowanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateAllowanceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Get<Domain.Entities.Allowance>()
                    .FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null && entity.IsDeleted == true)
        {
            throw new NotFoundException("Not Found Item " + request.Id);
        }

        entity.Name = request.Name;
        entity.AllowanceType = (Domain.Enums.AllowanceType)request.AllowanceType;
        entity.Amount = request.Amount;
        entity.Eligibility_Criteria = request.Eligibility_Criteria;
        entity.Requirements = request.Requirements;
        entity.LastModified = DateTime.Now;
        //entity.LastModifiedBy = 
        if (await _context.SaveChangesAsync(cancellationToken) == 0)
            throw new Exception();
            
        return Unit.Value;
    }
}