using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Identity;

namespace WebUI.Services.PayslipServices;

public interface IPayslipService
{
    Task<string> GrossToNetExperid(ApplicationUser user, DefaultConfig defaultConfig, List<DetailTaxIncome> taxIncome, List<Exchange> exchange, RegionalMinimumWage regional, InsuranceConfig insuranceConfig, DateTime tempNow, List<ShiftConfig> shiftConfig, EmployeeContract contract);
    Task<string> GrossToNetPending(ApplicationUser user, DefaultConfig defaultConfig, List<DetailTaxIncome> taxIncome, List<Exchange> exchange, RegionalMinimumWage regional, InsuranceConfig insuranceConfig, DateTime now, List<ShiftConfig> shiftConfig, EmployeeContract contract);
    Task<double> ExchangeFromNetToGross(ApplicationUser user, DefaultConfig defaultConfig, List<DetailTaxIncome> taxIncome, List<Exchange> exchanges, RegionalMinimumWage regional, InsuranceConfig insuranceConfig, DateTime tempNow, List<ShiftConfig> shiftConfig, EmployeeContract contract);
}
