using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.AnnualWorkingDays.Queries.GetList;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.AnnualWorkingDays.Queries.GetByRelatedObject;

public class GetAnnualByDayRequest : IRequest<Domain.Entities.AnnualWorkingDay>
{
    public DateTime Date { get; set; }
    public int Year { get; set; }


}
public class GetAnnualByDayRequestHandler : IRequestHandler<GetAnnualByDayRequest, Domain.Entities.AnnualWorkingDay>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetAnnualByDayRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.AnnualWorkingDay> Handle(GetAnnualByDayRequest request, CancellationToken cancellationToken)
    {

        var item = _context.Get<Domain.Entities.AnnualWorkingDay>().Where(x => x.IsDeleted == false && x.Day.Date == request.Date.Date).FirstOrDefault();
        return Task.FromResult(item);
    }
}

