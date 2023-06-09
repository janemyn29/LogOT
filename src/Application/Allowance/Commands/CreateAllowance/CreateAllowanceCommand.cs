using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.Allowance.Commands.CreateAllowance;
public class CreateAllowanceCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public int AllowanceType { get; set; }
    public float Amount { get; set; }
    public string Eligibility_Criteria { get; set; }
    public string Requirements { get; set; }
}

public class CreateAllowanceCommandHandler : IRequestHandler<CreateAllowanceCommand, Guid>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public CreateAllowanceCommandHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Guid> Handle(CreateAllowanceCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Allowance()
        {
            Name = request.Name,
            AllowanceType = (AllowanceType)request.AllowanceType,
            Amount = request.Amount,
            Eligibility_Criteria = request.Eligibility_Criteria,
            Requirements = request.Requirements,
        };

        _applicationDbContext.Get<Domain.Entities.Allowance>().Add(entity);
        if (await _applicationDbContext.SaveChangesAsync(cancellationToken) == 0)
        {
            throw new Exception();
        }

        return entity.Id;
    }
}
