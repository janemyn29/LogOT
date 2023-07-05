using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Identity;

namespace WebUI.Services.PayslipServices;

public interface IPayslipService
{
    Task<string> GrossToNet(ApplicationUser user, DefaultConfig defaultConfig, List<DetailTaxIncome> taxIncome, List<Exchange> exchange, RegionalMinimumWage regional, InsuranceConfig insuranceConfig, DateTime now, List<ShiftConfig> shiftConfig, EmployeeContract contract);
}
