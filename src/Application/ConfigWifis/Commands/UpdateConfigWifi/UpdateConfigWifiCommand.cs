
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.ConfigWifis.Commands.UpdateConfigWifi;
public class UpdateConfigWifiCommand : IRequest
{
    public Guid Id { get; set; }
    public string NameWifi { get; set; }
    public string WifiIPv4 { get; set; }
}

public class UpdateConfigWifiCommandHandler : IRequestHandler<UpdateConfigWifiCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public UpdateConfigWifiCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(UpdateConfigWifiCommand request, CancellationToken cancellationToken)
    {
        var check = await _context.Get<Domain.Entities.ConfigWifi>()
                    .FindAsync(new object[] { request.Id }, cancellationToken);

        if (check == null || check.IsDeleted == true)
        {
            throw new NotFoundException("Không tìm thấy IP " + request.WifiIPv4);
        }

        check.NameWifi = request.NameWifi;
        check.WifiIpv4 = request.WifiIPv4;

        if (await _context.SaveChangesAsync(cancellationToken) == 0)
            throw new Exception();

        return Unit.Value;
    }
}
