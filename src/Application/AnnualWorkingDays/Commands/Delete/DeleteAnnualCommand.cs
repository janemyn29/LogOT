using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.AnnualWorkingDays.Commands.Delete;
public record DeleteAnnualCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}

public class DeleteAnnualCommandHandler : IRequestHandler<DeleteAnnualCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteAnnualCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteAnnualCommand request, CancellationToken cancellationToken)
    {
        var CurrentCity = _context.Get<Domain.Entities.AnnualWorkingDay>().Where(x => x.IsDeleted == false && x.Id == request.Id).FirstOrDefault();

        if (CurrentCity == null)
        {
            throw new NotFoundException("Không tìm thấy hợp đồng mà bạn yêu cầu!");
        }else if (CurrentCity.Day.Date < DateTime.Now.Date)
        {
            throw new NotFoundException("Không thể xóa những ngày trong quá khứ!");

        }
        CurrentCity.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

