using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.Degree.Commands.CreateDegree;
public class CreateDegreeCommand : IRequest<Guid>
{
   public CreateDegreeViewModel createDegreeViewModel;
}

public class CreateDegreeCommandHandler : IRequestHandler<CreateDegreeCommand, Guid>
{ 
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    public CreateDegreeCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateDegreeCommand request, CancellationToken cancellationToken)
    {
        var degreeObject = _mapper.Map<Domain.Entities.Degree>(request.createDegreeViewModel);
        _applicationDbContext.Get<Domain.Entities.Degree>().Add(degreeObject);

        if (await _applicationDbContext.SaveChangesAsync(cancellationToken) == 0)
            throw new Exception();
        return degreeObject.Id;
    }
}
