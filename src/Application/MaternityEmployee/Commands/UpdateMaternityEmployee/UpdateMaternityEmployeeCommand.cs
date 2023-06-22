

using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.MaternityEmployee.Commands.UpdateMaternityEmployee;
public class UpdateMaternityEmployeeCommand : IRequest
{
    public UpdateMaternityEmployeeViewModel updateMaternityEmployeeView;
}

public class UpdateMaternityEmployeeCommandHandler : IRequestHandler<UpdateMaternityEmployeeCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateMaternityEmployeeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateMaternityEmployeeCommand request, CancellationToken cancellationToken)
    { 
        var entity = await _context.Get<Domain.Entities.MaternityEmployee>()
            .FindAsync(new object[] {request.updateMaternityEmployeeView.Id}, cancellationToken);

        if (entity == null || entity.IsDeleted == true)
        {
            throw new NotFoundException("Không tìm thấy ID " + request.updateMaternityEmployeeView.Id);
        }

        var degreeObject = _mapper.Map(request.updateMaternityEmployeeView, entity);

        if (await _context.SaveChangesAsync(cancellationToken) == 0)
            throw new Exception();

        return Unit.Value;
    }
}
