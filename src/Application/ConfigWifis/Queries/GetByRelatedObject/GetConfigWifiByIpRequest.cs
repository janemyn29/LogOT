using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;

namespace mentor_v1.Application.ConfigWifis.Queries.GetByRelatedObject;
public class GetConfigWifiByIpRequest : IRequest<Domain.Entities.ConfigWifi>
{
    public string Ip { get; set; }

}

// IRequestHandler<request type, return type>
public class GetConfigWifiByIpRequestHandler : IRequestHandler<GetConfigWifiByIpRequest, Domain.Entities.ConfigWifi>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetConfigWifiByIpRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.ConfigWifi> Handle(GetConfigWifiByIpRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var item = _context.Get<Domain.Entities.ConfigWifi>().Where(x => x.IsDeleted == false && x.WifiIpv4 == request.Ip).FirstOrDefault();
        if (item == null)
        {
            throw new NotFoundException("Không tìm thấy hợp đồng mà bạn yêu cầu!");
        }

        // Paginate data
        return Task.FromResult(item); //Task.CompletedTask;
    }
}




