using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.RegionalMinimumWage.Commands;
public record UpdateWageCommand : IRequest
{
    public Guid Id { get; set; }
    public double Amount { get; set; }
    public RegionType RegionType { get; set; }
}

public class UpdateWageCommandHandler : IRequestHandler<UpdateWageCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateWageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateWageCommand request, CancellationToken cancellationToken)
    {

        var current = _context.Get<Domain.Entities.RegionalMinimumWage>().Where(x => x.Id == request.Id && x.IsDeleted == false).FirstOrDefault();

        if (current == null)
        {
            throw new NotFoundException("Không tìm thấy cấu hình lương tối thiểu bạn yêu cầu!");
        }
        current.Amount = request.Amount;
        current.RegionType = request.RegionType;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

