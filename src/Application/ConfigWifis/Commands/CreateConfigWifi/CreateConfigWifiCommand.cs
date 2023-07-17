
using System.Data.Entity;
using MediatR;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.ConfigWifis.Commands.CreateConfigWifi;
public class CreateConfigWifiCommand : IRequest
{
    public string NameWifi { get; set; }
    public string WifiIPv4 { get; set; }
}

public class CreateConfigWifiCommandHandler : IRequestHandler<CreateConfigWifiCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateConfigWifiCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(CreateConfigWifiCommand request, CancellationToken cancellationToken)
    {

        var check = _context.Get<Domain.Entities.ConfigWifi>().Where(x => x.WifiIpv4.Equals(request.WifiIPv4) && x.IsDeleted == false).FirstOrDefault();

        if(check != null)
            throw new Exception("WifiIPv4: " + request.WifiIPv4 + " đã xuất hiện.");

        var entity = new Domain.Entities.ConfigWifi()
        { 
            NameWifi = request.NameWifi,
            WifiIpv4 = request.WifiIPv4
        };

        _context.Get<Domain.Entities.ConfigWifi>().Add(entity);
        if (await _context.SaveChangesAsync(cancellationToken) == 0)
        {
            throw new Exception();
        }
        return Unit.Value;
    }
}
