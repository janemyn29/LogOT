using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.Coefficients.Commands;
public record UpdateCoefficientCommand : IRequest
{
    public Guid Id { get; set; }
    public double AmountCoefficient { get; set; }
}

public class UpdateCoefficientCommandHandler : IRequestHandler<UpdateCoefficientCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCoefficientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCoefficientCommand request, CancellationToken cancellationToken)
    {

        var current = _context.Get<Domain.Entities.Coefficient>().Where(x => x.Id == request.Id && x.IsDeleted == false).FirstOrDefault();

        if (current == null)
        {
            throw new NotFoundException("Không tìm thấy cấu hình hệ số lương tăng ca mà bạn yêu cầu!");
        }
        current.AmountCoefficient = request.AmountCoefficient;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

