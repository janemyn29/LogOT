using MediatR;
using mentor_v1.Application.AnnualWorkingDays.Queries.GetByRelatedObject;
using mentor_v1.Application.Attendance.Queries.GetAttendanceWithRelativeObject;
using mentor_v1.Application.Department.Queries.GetDepartment;
using mentor_v1.Application.DepartmentAllowance.Queries.GetDepartmentAllowanceWithRelativeObject;
using mentor_v1.Application.Dependent.Queries;
using mentor_v1.Application.Payday.Queries;
using mentor_v1.Application.Payslip.Queries.GetList;
using mentor_v1.Application.ShiftConfig.Queries;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace WebUI.Services.PayslipServices;

public class PayslipService : IPayslipService
{
    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;

    public PayslipService(IMediator mediator, UserManager<ApplicationUser> userManager)
    {
        _mediator = mediator;
        _userManager = userManager;
    }


    public async Task<string> CaculatorPayslip(ApplicationUser user, DefaultConfig defaultConfig, DetailTaxIncome taxIncome, Exchange exchange, RegionalMinimumWage regional, InsuranceConfig insuranceConfig)
    {

        return null;
    }
    public async Task<string> GrossToNet(ApplicationUser user, DefaultConfig defaultConfig, List<DetailTaxIncome> taxIncome, List<Exchange> exchange, RegionalMinimumWage regional, InsuranceConfig insuranceConfig , DateTime tempNow, List<ShiftConfig> shiftConfig,EmployeeContract contract)
    {
        //lấy ngày trả lương trước hoặc ngày bắt đầu hợp đồng hoặc payday

        var now = tempNow.Date;
        //
        var yesterday = now.AddDays(-1);

        double salaryTax = 0;//Mức lương đống BH
        double BHXH_Emp_Amount = 0;
        double BHYT_Emp_Amount = 0;
        double BHTN_Emp_Amount = 0;

        double BHXH_Comp_Amount = 0;
        double BHYT_Comp_Amount = 0;
        double BHTN_Comp_Amount = 0;
        double BHCD_Comp_Amount = 0;

        double PersonalDeduction = 0;
        double DependanceDeduction = 0;



        if (contract.InsuranceType == InsuranceType.BaseOnMinimum) //dựa trên mức tối thiểu
        {
            salaryTax = regional.Amount; // mức lương đóng bh = mức lương tối thiểu của vùng;
        }
        else if (contract.InsuranceType == InsuranceType.BaseOnSalary)
        {
            salaryTax = (double)contract.InsuranceAmount; // mức lương đóng bh = mức đóng bh trong hợp đồng;
        }
        else
        {
            if (defaultConfig.BaseSalary * defaultConfig.InsuranceLimit < contract.BasicSalary)
            {
                salaryTax = defaultConfig.BaseSalary * defaultConfig.InsuranceLimit; // mức lương đóng bh = mức đóng bh tối đa;
            }
            else
            {
                salaryTax = (double)contract.BasicSalary;
            }
        }

        //KTra ngày tính lương.
        int payday = 1;
        if (now.Day != payday)
        {
            throw new Exception("Tính lương chỉ có thể thực hiện vào ngày 1 hàng tháng!");
        }
        DateTime lastPay = now.AddMonths(-1); //này bắt đầu tisng lương = lastPay + 1 ngày;
        var listPayday = await _mediator.Send(new GetListPaydayRequest { });
        var lastPayday = listPayday.OrderByDescending(x=>x.PaymentDay).FirstOrDefault();
        if(lastPayday != null)
        {
            if (now.Date <= lastPayday.PaymentDay.Date || now.Date <= contract.StartDate)
            {
                throw new Exception("Ngày tính lương không thể trùng với ngày trả lương lần trước hoặc ngày bắt đầu hợp đồng");
            }
        }
        var listAnnualDay = await _mediator.Send(new GetListAnnualByDayToDayRequest { FromDate = lastPay, ToDate = yesterday });
        double defaultWorkingHour = 0;
        double totalWorkingHour = 0;
        double OTHour = 0;

        var listAttendance = await _mediator.Send(new GetListAttendanceByUserNoVm { UserId = user.Id });
        var finalList = listAttendance.Where(x => x.Day.Date >= lastPay.Date && x.Day.Date <= yesterday.Date).ToList();

        foreach (var item in listAnnualDay)
        {
            var morningAttendance = finalList.Where(x=>x.Day.Date == item.Day.Date && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault();
            var affternoonAttendance = finalList.Where(x => x.Day.Date == item.Day.Date && x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault();

            var morning = shiftConfig.Where(x => x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault();
            var afternoon = shiftConfig.Where(x => x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault();
            var morningHour = CountHour((DateTime)morning.StartTime, (DateTime)morning.EndTime);
            var afternoonHour = CountHour((DateTime)afternoon.StartTime, (DateTime)afternoon.EndTime);

            if (item.ShiftType == ShiftType.Full)
            {
                defaultWorkingHour = defaultWorkingHour + morningHour + afternoonHour;
                if (morningAttendance != null && morningAttendance.TimeWork != null)
                {
                    totalWorkingHour = (double)(totalWorkingHour + morningAttendance.TimeWork);
                }
                if (affternoonAttendance != null && affternoonAttendance.TimeWork != null)
                {
                    totalWorkingHour = (double)(totalWorkingHour + affternoonAttendance.TimeWork);
                    OTHour = (double)(OTHour + affternoonAttendance.OverTime);
                }
            }
            else if (item.ShiftType == ShiftType.Morning)
            {
                defaultWorkingHour = defaultWorkingHour + morningHour;
                if (morningAttendance != null && morningAttendance.TimeWork != null)
                {
                    totalWorkingHour = (double)(totalWorkingHour + morningAttendance.TimeWork);
                }
                if (affternoonAttendance != null && affternoonAttendance.OverTime != null)
                {
                    OTHour = (double)(OTHour + affternoonAttendance.OverTime);
                }
            }
            else if (item.ShiftType == ShiftType.Afternoon)
            {
                defaultWorkingHour = defaultWorkingHour + afternoonHour;
                if (morningAttendance != null && morningAttendance.OverTime != null)
                {
                    OTHour = (double)(OTHour + morningAttendance.OverTime);
                }
                if (affternoonAttendance != null && affternoonAttendance.TimeWork != null)
                {
                    totalWorkingHour = (double)(totalWorkingHour + affternoonAttendance.TimeWork);
                    OTHour = (double)(OTHour + affternoonAttendance.OverTime);
                }
            }
            else if (item.ShiftType == ShiftType.Afternoon)
            {
                defaultWorkingHour = defaultWorkingHour + afternoonHour;
                if (morningAttendance != null && morningAttendance.OverTime != null)
                {
                    OTHour = (double)(OTHour + morningAttendance.OverTime);
                }
                if (affternoonAttendance != null && affternoonAttendance.OverTime != null)
                {
                    OTHour = (double)(OTHour + affternoonAttendance.OverTime);
                }
            }
        }

        //lấy giờ làm giảm cho người có thai
        double MaternityHour = 0;
        if (user.IsMaternity)
        {
            int days = CountDay(lastPay.Date, yesterday.Date);
            if (totalWorkingHour + days >= defaultWorkingHour)
            {
                MaternityHour = defaultWorkingHour - totalWorkingHour;
            }
            else
            {
                MaternityHour = days;
            }
        }

        var finalHour = totalWorkingHour + MaternityHour;
        double salaryPerHour =(double)(contract.BasicSalary / defaultWorkingHour);
        double totalSalary = Math.Round(salaryPerHour * finalHour);
        double leaveDeduction = (double)(contract.BasicSalary - totalSalary);
        if(leaveDeduction <=1)
        {
            leaveDeduction = 0;
        }

        //tính bảo hiểm:
        BHXH_Emp_Amount = Math.Round((salaryTax * insuranceConfig.BHXH_Emp / 100));
        BHYT_Emp_Amount = Math.Round((salaryTax * insuranceConfig.BHYT_Emp / 100));
        BHTN_Emp_Amount = Math.Round((salaryTax * insuranceConfig.BHTN_Emp / 100));

        var totalBH_Emp = BHYT_Emp_Amount + BHTN_Emp_Amount + BHXH_Emp_Amount;

        //thu nhập trước thuế = 
        var TNTT = contract.BasicSalary - totalBH_Emp - leaveDeduction; ///tính lại lương

        //nếu có thêm giảm trừ gia cảnh bản thân
        if (contract.isPersonalTaxDeduction)
        {
            PersonalDeduction = Math.Round(defaultConfig.PersonalTaxDeduction);
        }


        //tính người phụ thuộc và giảm trừ người phụ thuộc
        var listDependance = await _mediator.Send(new GetDependantByUserIdRequest { UserId = user.Id });
        int numOfDependance = 0;
        if (listDependance != null)
        {
            numOfDependance = listDependance.Count();
        }

        if (numOfDependance>0)
        {
            DependanceDeduction = Math.Round(defaultConfig.DependentTaxDeduction * numOfDependance);
        }

        // thu nhập chịu thuế = thu nhập trước thuế - giảm trừ gia cảnh bản thân và giảm trừ người phụ thuộc
        var TNCT =(double) TNTT - PersonalDeduction - DependanceDeduction;
        if (TNCT < 0)
        {
            TNCT = 0;
        }
        DetailTaxIncome tax;
        var listTax = taxIncome.Where(x => x.Muc_chiu_thue_From < TNCT ).ToList();
        if (listTax.Count >1)
        {
            tax = taxIncome.Where(x => x.Muc_chiu_thue_From < TNCT && x.Muc_chiu_thue_To >= TNCT).FirstOrDefault();
        }
        else
        {
            tax = listTax.FirstOrDefault();
        }
        // double TotalTaxIncome =Math.Round( (TNCT * tax.Thue_suat/100) - tax.He_so_tru);
        double TotalTaxIncome = 0;
        List <DetailTax> DetailTaxs = new List<DetailTax>();
        foreach (var item in taxIncome.OrderBy(x=>x.Thue_suat))
        {
            var taxDetail = new DetailTax();
            if(item.Thue_suat < tax.Thue_suat)
            {
                taxDetail.Muc_chiu_thue_From = item.Muc_chiu_thue_From;
                taxDetail.Muc_chiu_thue_To =item.Muc_chiu_thue_To;
                taxDetail.Thue_suat = item.Thue_suat;
                if(taxDetail.Muc_chiu_thue_To != null)
                {
                    taxDetail.TaxAmount = Math.Round((double)((item.Muc_chiu_thue_To - item.Muc_chiu_thue_From) * item.Thue_suat / 100));
                }
            }
            else if (item.Thue_suat > tax.Thue_suat)
            {
                taxDetail.Muc_chiu_thue_From = item.Muc_chiu_thue_From;
                taxDetail.Muc_chiu_thue_To = item.Muc_chiu_thue_To;
                taxDetail.Thue_suat = item.Thue_suat;
                taxDetail.TaxAmount =0;
            }
            else
            {
                taxDetail.Muc_chiu_thue_From = item.Muc_chiu_thue_From;
                taxDetail.Muc_chiu_thue_To = item.Muc_chiu_thue_To;
                taxDetail.Thue_suat = item.Thue_suat;
                taxDetail.TaxAmount = Math.Round((double)((TNCT - item.Muc_chiu_thue_From) * item.Thue_suat / 100));
            }
            TotalTaxIncome = TotalTaxIncome + taxDetail.TaxAmount;
            DetailTaxs.Add(taxDetail);
        }


        //tính cấc khoản trợ cấp , phụ cấp + tính các khoản
        var listAllowance = contract.AllowanceEmployees;
        double  totalAllowance= 0;
        if (listAllowance != null)
        {
            foreach (var item in listAllowance)
            {
                totalAllowance = totalAllowance + item.Allowance.Amount;
            }
        }
        double totalDepartmentAllowance = 0;

        var departmentAllowance = await _mediator.Send(new GetDepartmentAllowanceByDepartmentIdRequest { Id = user.Position.DepartmentId });
        if(departmentAllowance != null)
        {
            foreach (var item in departmentAllowance)
            {
                totalDepartmentAllowance = totalDepartmentAllowance + item.Subsidize.Amount;
            }
        }

        double netSalary = TNCT - TotalTaxIncome + totalAllowance + totalDepartmentAllowance;

        return netSalary.ToString();
    }


    public int CountDay(DateTime fromDate , DateTime toDate)
    {

        // Tính số ngày giữa hai ngày
        TimeSpan khoangCach = toDate - fromDate;
        int soNgay = khoangCach.Days;
        return soNgay;
    }

    public double CountHour (DateTime fromDate , DateTime toDate)
    {
        // Tính khoảng thời gian giữa hai thời điểm
        TimeSpan khoangThoiGian = toDate - fromDate;

        // Chuyển đổi số giờ thành dạng double
        double soGio = khoangThoiGian.TotalHours;
        return soGio;
    }
}
