using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Dependent.Commands.UpdateDependent;
public class UpdateDependentCommand : IRequest
{
    public UpdateDependentViewModel _updateDependentViewModel;
}

public class UpdateDependentCommandHandler : IRequestHandler<UpdateDependentCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    public UpdateDependentCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(UpdateDependentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _applicationDbContext.Get<Domain.Entities.Dependent>()
            .FindAsync(new object[] { request._updateDependentViewModel.Id }, cancellationToken);

        if (entity == null || entity.IsDeleted == true)
        {
            throw new NotFoundException("Không tìm thấy ID " + request._updateDependentViewModel.Id);
        }

        var degreeObject = _mapper.Map(request._updateDependentViewModel, entity);

        if (await _applicationDbContext.SaveChangesAsync(cancellationToken) == 0)
            throw new Exception();

        return Unit.Value;
    }
}
