using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.DefaultConfig.Commands;
public record UpdateDefaultConfigCommand : IRequest
{
    public RegionType CompanyRegionType { get; set; }
    public double BaseSalary { get; set; }
    public double PersonalTaxDeduction { get; set; }
    public double DependentTaxDeduction { get; set; }
    public int InsuranceLimit { get; set; }
}

public class UpdateDefaultConfigCommandHandler : IRequestHandler<UpdateDefaultConfigCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDefaultConfigCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateDefaultConfigCommand request, CancellationToken cancellationToken)
    {
        var current = _context.Get<Domain.Entities.DefaultConfig>().Where(x => x.IsDeleted == false).FirstOrDefault();

        if (current == null)
        {
            throw new NotFoundException("Không tìm thấy cấu hình bạn yêu cầu!");
        }
        current.CompanyRegionType = request.CompanyRegionType;
        current.BaseSalary = request.BaseSalary;
        current.PersonalTaxDeduction = request.PersonalTaxDeduction;
        current.DependentTaxDeduction = request.DependentTaxDeduction;
        current.InsuranceLimit = request.InsuranceLimit;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}


