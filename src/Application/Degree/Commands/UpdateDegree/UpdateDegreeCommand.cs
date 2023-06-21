using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Degree.Commands.UpdateDegree;
public class UpdateDegreeCommand : IRequest
{
    public UpdateDegreeViewModel _updateDegreeViewModel;
}

public class UpdateDegreeCommandHandler : IRequestHandler<UpdateDegreeCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    public UpdateDegreeCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(UpdateDegreeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _applicationDbContext.Get<Domain.Entities.Degree>()
            .FindAsync(new object[] { request._updateDegreeViewModel.Id }, cancellationToken);

        if (entity == null || entity.IsDeleted == true) 
        {
            throw new NotFoundException("Không tìm thấy ID " + request._updateDegreeViewModel.Id);
        }

        var degreeObject = _mapper.Map(request._updateDegreeViewModel, entity);

        if (await _applicationDbContext.SaveChangesAsync(cancellationToken) == 0)
            throw new Exception();

        return Unit.Value;
    }
}