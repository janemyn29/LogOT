using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.DefaultConfig.Queries.Get;
public class GetDefaultConfigRequestView : IRequest<DefaultConfigViewModel>
{
}

// IRequestHandler<request type, return type>
public class GetDefaultConfigRequestViewHandler : IRequestHandler<GetDefaultConfigRequestView, DefaultConfigViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetDefaultConfigRequestViewHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<DefaultConfigViewModel> Handle(GetDefaultConfigRequestView request, CancellationToken cancellationToken)
    {
        // get categories
        var EmpContract = _context.Get<Domain.Entities.DefaultConfig>().Where(x => x.IsDeleted == false);
        if (EmpContract == null)
        {
            throw new NotFoundException("Không tìm thấy cấu hình lương mà bạn yêu cầu!");
        }
        var map = _mapper.ProjectTo<DefaultConfigViewModel>(EmpContract).FirstOrDefault();
        return Task.FromResult(map); //Task.CompletedTask;
    }
}

