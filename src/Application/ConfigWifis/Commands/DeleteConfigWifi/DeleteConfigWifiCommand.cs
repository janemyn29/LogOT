
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.ConfigWifis.Commands.DeleteConfigWifi;
public class DeleteConfigWifiCommand : IRequest<bool>
{
    public string IPv4 { get; set; }
}

public class DeleteConfigWifiCommandHandler : IRequestHandler<DeleteConfigWifiCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteConfigWifiCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteConfigWifiCommand request, CancellationToken cancellationToken)
    {
        var ip = await _context.Get<Domain.Entities.ConfigWifi>().Where(x => x.WifiIpv4.Equals(request.IPv4)).FirstOrDefaultAsync();
        if (ip == null || ip.IsDeleted == true)
        {
            throw new NotFoundException("Không tìm thấy ID " + request.IPv4 + ". Xoá thất bại.");
        }
        ip.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
