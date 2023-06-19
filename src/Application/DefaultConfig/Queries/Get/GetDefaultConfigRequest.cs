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

namespace mentor_v1.Application.DefaultConfig.Queries.Get;
public class GetDefaultConfigRequest : IRequest<Domain.Entities.DefaultConfig>
{
}

// IRequestHandler<request type, return type>
public class GetDefaultConfigRequestHandler : IRequestHandler<GetDefaultConfigRequest, Domain.Entities.DefaultConfig>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetDefaultConfigRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.DefaultConfig> Handle(GetDefaultConfigRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var EmpContract = _context.Get<Domain.Entities.DefaultConfig>().Where(x => x.IsDeleted == false).FirstOrDefault();
        if (EmpContract == null)
        {
            throw new NotFoundException("Không tìm thấy cấu hình lương mà bạn yêu cầu!");
        }
        return Task.FromResult(EmpContract); //Task.CompletedTask;
    }
}
