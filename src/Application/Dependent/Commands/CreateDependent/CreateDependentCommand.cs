using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Packaging.Ionic.Zip;

namespace mentor_v1.Application.Dependent.Commands.CreateDependent;
public class CreateDependentCommand : IRequest<Guid>
{
    public CreateDependentViewModel createDependentViewModel;
}

public class CreateCreateDependentCommandhandler : IRequestHandler<CreateDependentCommand, Guid>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;

    public CreateCreateDependentCommandhandler(IApplicationDbContext applicationDbContext, IMapper mapper, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _userManager = userManager;
    }
    public async Task<Guid> Handle(CreateDependentCommand request, CancellationToken cancellationToken)
    {
        var dependent = _mapper.Map<Domain.Entities.Dependent>(request.createDependentViewModel);
        dependent.AcceptanceType = Domain.Enums.AcceptanceType.Request;
        if (await _userManager.Users.Where(x => x.Id.Equals(request.createDependentViewModel.ApplicationUserId)).AsNoTracking().FirstOrDefaultAsync() == null)
            throw new ArgumentNullException();

        _applicationDbContext.Get<Domain.Entities.Dependent>().Add(dependent);

        if (await _applicationDbContext.SaveChangesAsync(cancellationToken) == 0)
            throw new Exception();
        return dependent.Id;
    }
}
