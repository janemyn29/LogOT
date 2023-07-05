using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers;
public class EnumController : ApiControllerBase
{

    [HttpGet("AcceptanceType")]
    public async Task<IActionResult> AcceptanceType()
    {
        List<EnumModel> enums = ((AcceptanceType[])Enum.GetValues(typeof(AcceptanceType))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("ActionType")]
    public async Task<IActionResult> ActionType()
    {
        List<EnumModel> enums = ((ActionType[])Enum.GetValues(typeof(ActionType))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("AllowanceType")]
    public async Task<IActionResult> AllowanceType()
    {
        List<EnumModel> enums = ((AllowanceType[])Enum.GetValues(typeof(AllowanceType))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("ContractType")]
    public async Task<IActionResult> ContractType()
    {
        List<EnumModel> enums = ((ContractType[])Enum.GetValues(typeof(ContractType))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("DefaultStatus")]
    public async Task<IActionResult> DefaultStatus()
    {
        List<EnumModel> enums = ((DefaultStatus[])Enum.GetValues(typeof(DefaultStatus))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("DegreeType")]
    public async Task<IActionResult> DegreeType()
    {
        List<EnumModel> enums = ((DegreeType[])Enum.GetValues(typeof(DegreeType))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("EmployeeContractStatus")]
    public async Task<IActionResult> EmployeeContractStatus()
    {
        List<EnumModel> enums = ((EmployeeContractStatus[])Enum.GetValues(typeof(EmployeeContractStatus))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("FileType")]
    public async Task<IActionResult> FileType()
    {
        List<EnumModel> enums = ((FileType[])Enum.GetValues(typeof(FileType))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("GenderType")]
    public async Task<IActionResult> GenderType()
    {
        List<EnumModel> enums = ((GenderType[])Enum.GetValues(typeof(GenderType))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("InsuranceType")]
    public async Task<IActionResult> InsuranceType()
    {
        List<EnumModel> enums = ((InsuranceType[])Enum.GetValues(typeof(InsuranceType))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("LeaveShift")]
    public async Task<IActionResult> LeaveShift()
    {
        List<EnumModel> enums = ((LeaveShift[])Enum.GetValues(typeof(LeaveShift))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("LevelEnum")]
    public async Task<IActionResult> LevelEnum()
    {
        List<EnumModel> enums = ((LevelEnum[])Enum.GetValues(typeof(LevelEnum))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("PaymentHistoryStatus")]
    public async Task<IActionResult> PaymentHistoryStatus()
    {
        List<EnumModel> enums = ((PaymentHistoryStatus[])Enum.GetValues(typeof(PaymentHistoryStatus))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("PositionLevel")]
    public async Task<IActionResult> PositionLevel()
    {
        List<EnumModel> enums = ((PositionLevel[])Enum.GetValues(typeof(PositionLevel))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("RegionType")]
    public async Task<IActionResult> RegionType()
    {
        List<EnumModel> enums = ((RegionType[])Enum.GetValues(typeof(RegionType))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("LogStatus")]
    public async Task<IActionResult> LogStatus()
    {
        List<EnumModel> enums = ((LogStatus[])Enum.GetValues(typeof(LogStatus))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("SalaryType")]
    public async Task<IActionResult> SalaryType()
    {
        List<EnumModel> enums = ((SalaryType[])Enum.GetValues(typeof(SalaryType))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("ShiftEnum")]
    public async Task<IActionResult> ShiftEnum()
    {
        List<EnumModel> enums = ((ShiftEnum[])Enum.GetValues(typeof(ShiftEnum))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("ShiftType")]
    public async Task<IActionResult> ShiftType()
    {
        List<EnumModel> enums = ((ShiftType[])Enum.GetValues(typeof(ShiftType))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("TypeDate")]
    public async Task<IActionResult> TypeDate()
    {
        List<EnumModel> enums = ((TypeDate[])Enum.GetValues(typeof(TypeDate))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }

    [HttpGet("WorkStatus")]
    public async Task<IActionResult> WorkStatus()
    {
        List<EnumModel> enums = ((WorkStatus[])Enum.GetValues(typeof(WorkStatus))).Select(c => new EnumModel() { Value = (int)c, Display = c.ToString() }).ToList();
        return Ok(enums);
    }
}
