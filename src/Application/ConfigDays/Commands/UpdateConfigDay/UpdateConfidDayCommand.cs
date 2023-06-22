using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.ConfigDays.Commands.UpdateConfigDay;

public record UpdateConfidDayCommand : IRequest
{
    public ShiftType Normal { get; set; }
    public ShiftType Saturday { get; set; }
    public ShiftType Sunday { get; set; }
    public ShiftType Holiday { get; set; }
}

public class UpdateConfidDayCommandHandler : IRequestHandler<UpdateConfidDayCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateConfidDayCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateConfidDayCommand request, CancellationToken cancellationToken)
    {
        var current = _context.Get<Domain.Entities.ConfigDay>().Where(x=> x.IsDeleted == false).FirstOrDefault();

        if (current == null)
        {
            throw new NotFoundException("Không tìm thấy cấu hình bạn yêu cầu!");
        }
        current.Normal = request.Normal;
        current.Saturday = request.Saturday;
        current.Sunday = request.Sunday;
        current.Holiday = request.Holiday;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

