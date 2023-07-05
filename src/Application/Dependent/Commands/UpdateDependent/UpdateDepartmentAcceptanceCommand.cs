using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Dependent.Commands.UpdateDependent;
public class UpdateDepartmentAcceptanceCommand : IRequest
{
    public Guid Id { get; set; }
    public AcceptanceType AcceptanceType { get; set; }
}

public class UpdateDepartmentAcceptanceCommandHandler : IRequestHandler<UpdateDepartmentAcceptanceCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    public UpdateDepartmentAcceptanceCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(UpdateDepartmentAcceptanceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _applicationDbContext.Get<Domain.Entities.Dependent>().Where(x=>x.IsDeleted == false && x.Id == request.Id ).FirstOrDefaultAsync();

        if (entity == null || entity.IsDeleted == true)
        {
            throw new NotFoundException("Không tìm thấy ID " + request.Id);
        }
        entity.AcceptanceType = request.AcceptanceType;

        if (await _applicationDbContext.SaveChangesAsync(cancellationToken) == 0)
            throw new Exception();

        return Unit.Value;
    }
}

