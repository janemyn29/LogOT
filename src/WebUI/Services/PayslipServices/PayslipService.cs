using MediatR;
using mentor_v1.Application.AnnualWorkingDays.Queries.GetByRelatedObject;
using mentor_v1.Application.Attendance.Queries.GetAttendanceWithRelativeObject;
using mentor_v1.Application.Department.Queries.GetDepartment;
using mentor_v1.Application.DepartmentAllowance.Queries.GetDepartmentAllowanceWithRelativeObject;
using mentor_v1.Application.Dependent.Queries;
using mentor_v1.Application.DetailTax;
using mentor_v1.Application.Payday.Queries;
using mentor_v1.Application.Payslip.Commands.Create;
using mentor_v1.Application.Payslip.Queries;
using mentor_v1.Application.Payslip.Queries.GetList;
using mentor_v1.Application.ShiftConfig.Queries;
using mentor_v1.Application.Subsidize.Queries.GetSubsidizeWithRelativeObject;
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
    public async Task<string> GrossToNetPending(ApplicationUser user, DefaultConfig defaultConfig, List<DetailTaxIncome> taxIncome, List<Exchange> exchange, RegionalMinimumWage regional, InsuranceConfig insuranceConfig, DateTime tempNow, List<ShiftConfig> shiftConfig, EmployeeContract contract)
    {
        //lấy ngày trả lương trước hoặc ngày bắt đầu hợp đồng hoặc payday
        double basicSalary = 0;
        if (contract.SalaryType == SalaryType.Gross)
        {
            basicSalary = (double)contract.BasicSalary;
        }
        else
        {
            basicSalary = await ExchangeFromNetToGross(user, defaultConfig, taxIncome, exchange, regional, insuranceConfig, tempNow, shiftConfig, contract);
        }
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

        double PersonalDeduction = 0;
        double DependanceDeduction = 0;



        if (contract.InsuranceType == InsuranceType.BaseOnMinimum) //dựa trên mức tối thiểu
        {
            salaryTax = regional.Amount; // mức lương đóng bh = mức lương tối thiểu của vùng;
        }
        else if (contract.InsuranceType == InsuranceType.BaseOnOtherAmount)
        {
            salaryTax = (double)contract.InsuranceAmount; // mức lương đóng bh = mức đóng bh trong hợp đồng;
        }
        else
        {
            if (defaultConfig.BaseSalary * defaultConfig.InsuranceLimit < basicSalary)
            {
                salaryTax = defaultConfig.BaseSalary * defaultConfig.InsuranceLimit; // mức lương đóng bh = mức đóng bh tối đa;
            }
            else
            {
                salaryTax = (double)basicSalary;
            }
        }
        DateTime lastPay = now.AddMonths(-1); //này bắt đầu tisng lương = lastPay + 1 ngày;
        var listPayday = await _mediator.Send(new GetListPaydayRequest { });
        var lastPayday = listPayday.OrderByDescending(x => x.PaymentDay).FirstOrDefault();
        if (lastPayday != null)
        {
            if (now.Date <= lastPayday.PaymentDay.Date || now.Date <= contract.StartDate.Value.Date)
            {
                throw new Exception("Ngày tính lương không thể trùng với ngày trả lương lần trước hoặc ngày bắt đầu hợp đồng");
            }
        }
        //nếu ngày bắt đầu của hợp đồng là giữa tháng.
        if (contract.StartDate.Value.Day > lastPay.Day && contract.StartDate.Value.Month == lastPay.Month && contract.StartDate.Value.Year == lastPay.Year)
        {
            lastPay = contract.StartDate.Value.Date;
        }
        var listAnnualDay = await _mediator.Send(new GetListAnnualByDayToDayRequest { FromDate = lastPay.Date, ToDate = yesterday.Date });
        double defaultWorkingHour = 0;
        double totalWorkingHour = 0;
        double OTHour = 0;
        double LeaveHour = 0;

        double CofieOT = 0;


        var listAttendance = await _mediator.Send(new GetListAttendanceByUserNoVm { UserId = user.Id });
        var finalList = listAttendance.Where(x => x.Day.Date >= lastPay.Date && x.Day.Date <= yesterday.Date).ToList();
        foreach (var item in listAnnualDay)
        {
            var morningAttendance = finalList.Where(x => x.Day.Date == item.Day.Date && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault();
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
                CofieOT = CofieOT + TotalOTWage(item, morningAttendance, affternoonAttendance);
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
                CofieOT = CofieOT + TotalOTWage(item, morningAttendance, affternoonAttendance);

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
                CofieOT = CofieOT + TotalOTWage(item, morningAttendance, affternoonAttendance);

            }
            else if (item.ShiftType == ShiftType.NotWork)
            {
                if (morningAttendance != null && morningAttendance.OverTime != null)
                {
                    OTHour = (double)(OTHour + morningAttendance.OverTime);
                }
                if (affternoonAttendance != null && affternoonAttendance.OverTime != null)
                {
                    OTHour = (double)(OTHour + affternoonAttendance.OverTime);
                }
                CofieOT = CofieOT + TotalOTWage(item, morningAttendance, affternoonAttendance);

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
        double salaryPerHour = (double)(basicSalary / defaultWorkingHour);
        double totalSalary = Math.Round(salaryPerHour * finalHour);
        double leaveDeduction = (double)(basicSalary - totalSalary);
        if (leaveDeduction <= 1)
        {
            leaveDeduction = 0;
        }
        LeaveHour = defaultWorkingHour - totalWorkingHour;
        if (LeaveHour == 0)
        {
            leaveDeduction = 0;
        }
        if(finalHour == 0)
        {
            salaryTax = 0;
        }
        //tính bảo hiểm:
        BHXH_Emp_Amount = Math.Round((salaryTax * insuranceConfig.BHXH_Emp / 100));
        BHYT_Emp_Amount = Math.Round((salaryTax * insuranceConfig.BHYT_Emp / 100));
        BHTN_Emp_Amount = Math.Round((salaryTax * insuranceConfig.BHTN_Emp / 100));

        var totalBH_Emp = BHYT_Emp_Amount + BHTN_Emp_Amount + BHXH_Emp_Amount;

        //thu nhập trước thuế = 
        var TNTT = basicSalary - totalBH_Emp - leaveDeduction; ///tính lại lương

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
            numOfDependance = listDependance.Where(x => x.AcceptanceType == AcceptanceType.Accept).Count();
        }

        if (numOfDependance > 0)
        {
            DependanceDeduction = Math.Round(defaultConfig.DependentTaxDeduction * numOfDependance);
        }

        // thu nhập chịu thuế = thu nhập trước thuế - giảm trừ gia cảnh bản thân và giảm trừ người phụ thuộc
        var TNCT = (double)TNTT - PersonalDeduction - DependanceDeduction;
        if (TNCT < 0)
        {
            TNCT = 0;
        }
        if (finalHour == 0)
        {
            TNCT = 0;
        }
        DetailTaxIncome tax = new DetailTaxIncome();
        if (TNCT == 0)
        {
            tax = taxIncome.OrderBy(x => x.Thue_suat).FirstOrDefault();
        }
        else
        {
            var listTax = taxIncome.Where(x => x.Muc_chiu_thue_From < TNCT).ToList();
            var temp = taxIncome.Where(x => x.Muc_chiu_thue_From < TNCT).ToList();
            if (listTax.Count() == taxIncome.Count())
            {
                tax = taxIncome.LastOrDefault();
            }
            else
            {
                tax = taxIncome.Where(x => x.Muc_chiu_thue_From < TNCT && x.Muc_chiu_thue_To >= TNCT).ToList().FirstOrDefault();
            }

        }

        // double TotalTaxIncome =Math.Round( (TNCT * tax.Thue_suat/100) - tax.He_so_tru);
        double TotalTaxIncome = 0;

        List<DetailTax> DetailTaxs = new List<DetailTax>();
        foreach (var item in taxIncome.OrderBy(x => x.Thue_suat))
        {
            var taxDetail = new DetailTax();
            if (item.Thue_suat < tax.Thue_suat)
            {
                taxDetail.Muc_chiu_thue_From = item.Muc_chiu_thue_From;
                taxDetail.Muc_chiu_thue_To = item.Muc_chiu_thue_To;
                taxDetail.Thue_suat = item.Thue_suat;
                if (taxDetail.Muc_chiu_thue_To != null)
                {
                    taxDetail.TaxAmount = Math.Round((double)((item.Muc_chiu_thue_To - item.Muc_chiu_thue_From) * item.Thue_suat / 100));
                }
            }
            else if (item.Thue_suat > tax.Thue_suat)
            {
                taxDetail.Muc_chiu_thue_From = item.Muc_chiu_thue_From;
                taxDetail.Muc_chiu_thue_To = item.Muc_chiu_thue_To;
                taxDetail.Thue_suat = item.Thue_suat;
                taxDetail.TaxAmount = 0;
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
        double totalAllowance = 0;
        if (listAllowance != null)
        {
            foreach (var item in listAllowance)
            {
                totalAllowance = totalAllowance + item.Allowance.Amount;
            }
        }
        double totalDepartmentAllowance = 0;

        var departmentAllowance = await _mediator.Send(new GetDepartmentAllowanceByDepartmentIdRequest { Id = user.Position.DepartmentId });
        if (departmentAllowance != null)
        {
            foreach (var item in departmentAllowance)
            {
                try
                {
                    var subsidize = await _mediator.Send(new GetSubsidizeByIdRequest { Id = item.SubsidizeId });
                    totalDepartmentAllowance = totalDepartmentAllowance + subsidize.Amount;
                }
                catch (Exception)
                {
                }
                
            }
        }
        /*        double OTWage = Math.Round(OTHour * salaryPerHour );*/
        double TNST = TNTT - TotalTaxIncome;
        double OTwage = Math.Round(CofieOT * salaryPerHour);
        double netSalary = (double)(TNTT - TotalTaxIncome + totalAllowance + totalDepartmentAllowance + OTwage);

        //tính bảo hiểm:
        BHXH_Comp_Amount = Math.Round((salaryTax * insuranceConfig.BHXH_Comp / 100));
        BHYT_Comp_Amount = Math.Round((salaryTax * insuranceConfig.BHYT_Comp / 100));
        BHTN_Comp_Amount = Math.Round((salaryTax * insuranceConfig.BHTN_Comp / 100));

        var totalBH_Comp = BHXH_Comp_Amount + BHYT_Comp_Amount + BHTN_Comp_Amount+ basicSalary;

        PayslipViewModel payslip = new PayslipViewModel();
        payslip.EmployeeContractId = contract.Id;
        payslip.Code = contract.ContractCode;
        payslip.FromTime = lastPay.Date;
        payslip.ToTime = yesterday.Date;
        payslip.PaydayCal = now;
        payslip.SalaryPerHour = salaryPerHour;
        payslip.Standard_Work_Hours = defaultWorkingHour;
        payslip.Actual_Work_Hours = totalWorkingHour;
        payslip.Ot_Hours = OTHour;
        payslip.Leave_Hours = LeaveHour;
        payslip.DefaultSalary = basicSalary;
        payslip.SalaryType = contract.SalaryType;
        payslip.InsuranceType = contract.InsuranceType;
        payslip.InsuranceAmount = salaryTax;
        payslip.isPersonalTaxDeduction = contract.isPersonalTaxDeduction;
        payslip.PersonalTaxDeductionAmount = PersonalDeduction;
        payslip.DependentTaxDeductionAmount = DependanceDeduction;
        payslip.RegionType = regional.RegionType;
        payslip.RegionMinimumWage = regional.Amount;
        payslip.IsMaternity = user.IsMaternity;
        payslip.MaternityHour = MaternityHour;

        payslip.BHXH_Emp_Amount = BHXH_Emp_Amount;
        payslip.BHYT_Emp_Amount = BHYT_Emp_Amount;
        payslip.BHTN_Emp_Amount = BHTN_Emp_Amount;
        payslip.BHXH_Emp_Percent = insuranceConfig.BHXH_Emp;
        payslip.BHYT_Emp_Percent = insuranceConfig.BHYT_Emp;
        payslip.BHTN_Emp_Percent = insuranceConfig.BHTN_Emp;

        payslip.BHXH_Comp_Amount = BHXH_Comp_Amount;
        payslip.BHYT_Comp_Amount = BHYT_Comp_Amount;
        payslip.BHTN_Comp_Amount = BHTN_Comp_Amount;
        payslip.BHXH_Comp_Percent = insuranceConfig.BHXH_Comp;
        payslip.BHYT_Comp_Percent = insuranceConfig.BHYT_Comp;
        payslip.BHTN_Comp_Percent = insuranceConfig.BHTN_Comp;

        payslip.TotalInsuranceEmp = totalBH_Emp;
        payslip.TotalInsuranceComp = totalBH_Comp;

        payslip.LeaveWageDeduction = leaveDeduction;
        payslip.TaxableSalary = TNCT;
        payslip.NumberOfDependent = numOfDependance;
        payslip.TotalDependentAmount = DependanceDeduction;
        payslip.TNTT = TNTT;
        payslip.TotalTaxIncome = TotalTaxIncome;

        payslip.AfterTaxSalary = TNST;
        payslip.TotalDepartmentAllowance = totalDepartmentAllowance;
        payslip.TotalContractAllowance = totalAllowance;

        payslip.OTWage = OTwage;
        payslip.FinalSalary = netSalary;
        payslip.Note = "Bảng lương nhân viên từ ngày " + lastPay.Date.ToString("dd/MM/yyy") + " đến ngày " + yesterday.ToString("dd/MM/yyyy") + ".";

        payslip.BankName = user.BankName;
        payslip.BankAcountName = user.BankAccountName;
        payslip.BankAcountNumber = user.BankAccountNumber;

        try
        {
            var payslipId = await _mediator.Send(new CreatePayslipCommand { payslip = payslip });
            foreach (var item in DetailTaxs)
            {
                DetailTax detail = new DetailTax();
                detail.PayslipId = payslipId;
                detail.Muc_chiu_thue_From = item.Muc_chiu_thue_From;
                detail.Muc_chiu_thue_To = item.Muc_chiu_thue_To;
                detail.Thue_suat = item.Thue_suat;
                detail.TaxAmount = item.TaxAmount;
                await _mediator.Send(new CreateDetailTaxCommand { DetailTax = detail });
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return "Tạo thành công";

    }

    public int CountDay(DateTime fromDate, DateTime toDate)
    {

        // Tính số ngày giữa hai ngày
        TimeSpan khoangCach = toDate - fromDate;
        int soNgay = khoangCach.Days;
        return soNgay;
    }

    public double CountHour(DateTime fromDate, DateTime toDate)
    {
        // Tính khoảng thời gian giữa hai thời điểm
        TimeSpan khoangThoiGian = toDate - fromDate;

        // Chuyển đổi số giờ thành dạng double
        double soGio = khoangThoiGian.TotalHours;
        return soGio;
    }

    public double TotalOTWage(AnnualWorkingDay item, Attendance morning, Attendance afternoon)
    {
        double OTNormal = 0;
        double OTSaturday = 0;
        double OTSunday = 0;
        double OTHoliday = 0;

        if (item.TypeDate == TypeDate.Normal)
        {
            if (morning != null && morning.OverTime != null)
            {
                OTNormal = morning.OverTime.Value;
            }
            if (afternoon != null && afternoon.OverTime != null)
            {
                OTNormal = OTNormal + afternoon.OverTime.Value;
            }

        }
        else if (item.TypeDate == TypeDate.Saturday)
        {
            if (morning != null && morning.OverTime != null)
            {
                OTSaturday = morning.OverTime.Value;
            }
            if (afternoon != null && afternoon.OverTime != null)
            {
                OTSaturday = OTSaturday + afternoon.OverTime.Value;
            }
        }
        else if (item.TypeDate == TypeDate.Sunday)
        {
            if (morning != null && morning.OverTime != null)
            {
                OTSunday = morning.OverTime.Value;
            }
            if (afternoon != null && afternoon.OverTime != null)
            {
                OTSunday = OTSunday + afternoon.OverTime.Value;
            }
        }
        else
        {
            if (morning != null && morning.OverTime != null)
            {
                OTHoliday = morning.OverTime.Value;
            }
            if (afternoon != null && afternoon.OverTime != null)
            {
                OTHoliday = OTHoliday + afternoon.OverTime.Value;
            }
        }

        double final = OTNormal * item.Coefficient.AmountCoefficient + OTSaturday * item.Coefficient.AmountCoefficient + OTSunday * item.Coefficient.AmountCoefficient + OTHoliday * item.Coefficient.AmountCoefficient;
        return final;
    }

    public async Task<double> ExchangeFromNetToGross(ApplicationUser user, DefaultConfig defaultConfig, List<DetailTaxIncome> taxIncome, List<Exchange> exchanges, RegionalMinimumWage regional, InsuranceConfig insuranceConfig, DateTime tempNow, List<ShiftConfig> shiftConfig, EmployeeContract contract)
    {
        if (contract.SalaryType == SalaryType.Gross)
        {
            throw new Exception("Lương Gross rồi!");
        }
        else
        {
            //tính người phụ thuộc.
            double DependanceDeduction = 0;
            var listDependance = await _mediator.Send(new GetDependantByUserIdRequest { UserId = user.Id });
            int numOfDependance = 0;
            if (listDependance != null)
            {
                numOfDependance = listDependance.Where(x => x.AcceptanceType == AcceptanceType.Accept).Count();
            }

            if (numOfDependance > 0)
            {
                DependanceDeduction = Math.Round(defaultConfig.DependentTaxDeduction * numOfDependance);
            }

            //Tính giảm trừ gia cảnh bản thân: 
            double PersonalDeduction = 0;

            if (contract.isPersonalTaxDeduction)
            {
                PersonalDeduction = Math.Round(defaultConfig.PersonalTaxDeduction);
            }

            double Gross = 0;
            //B1: tính thu nhập quay đổi:
            double TNQD = (double)(contract.BasicSalary - DependanceDeduction - PersonalDeduction);

            if (TNQD <= 0)
            {
                TNQD = 0;
            }
            Exchange exChange = new Exchange();
            List<Exchange> finalListEx = new List<Exchange>();

            if (TNQD <= 0)
            {
                exChange = exchanges.OrderBy(x => x.Thue_Suat).FirstOrDefault();
            }
            else
            {
                finalListEx = exchanges.Where(x => x.Muc_Quy_Doi_From < TNQD).ToList();
                if (finalListEx.Count() == exchanges.Count())
                {
                    exChange = finalListEx.LastOrDefault();
                }
                else
                {

                    exChange = exchanges.Where(x => x.Muc_Quy_Doi_To >= TNQD && x.Muc_Quy_Doi_From < TNQD).ToList().FirstOrDefault();
                }
            }

            double TNCT = Math.Round((TNQD - exChange.Giam_Tru) / exChange.Thue_Suat);
            double TNTT = 0;
            if (TNCT <= 0)
            {
                TNCT = 0;
                TNTT = (double)contract.BasicSalary;
            }
            else
            {
                TNTT = TNCT + PersonalDeduction + DependanceDeduction;
            }

            double salaryIns = 0;
            double InsCoe = (insuranceConfig.BHXH_Emp + insuranceConfig.BHYT_Emp + insuranceConfig.BHTN_Emp) / 100;
            double TotalBH = 0;

            if (contract.InsuranceType == InsuranceType.BaseOnMinimum)
            {
                salaryIns = regional.Amount;
                TotalBH = Math.Round(salaryIns * InsCoe);
                Gross = TNTT + TotalBH;
                return Gross;
            }
            else if (contract.InsuranceType == InsuranceType.BaseOnOtherAmount)
            {
                salaryIns = regional.Amount;
                TotalBH = Math.Round(salaryIns * InsCoe);
                Gross = TNTT + TotalBH;
                return Gross;

            }
            else
            {
                double tempGross = Math.Round(TNTT / (1 - InsCoe));
                if (tempGross > defaultConfig.BaseSalary * defaultConfig.InsuranceLimit)
                {
                    salaryIns = defaultConfig.BaseSalary * defaultConfig.InsuranceLimit;
                    TotalBH = Math.Round(salaryIns * InsCoe);
                    Gross = TNTT + TotalBH;
                    return Gross;
                }
                else
                {
                    Gross = tempGross;
                    return Gross;
                }
            }
        }

    }

    public async Task<string> GrossToNetExperid(ApplicationUser user, DefaultConfig defaultConfig, List<DetailTaxIncome> taxIncome, List<Exchange> exchange, RegionalMinimumWage regional, InsuranceConfig insuranceConfig, DateTime tempNow, List<ShiftConfig> shiftConfig, EmployeeContract contract)
    {
        //lấy ngày trả lương trước hoặc ngày bắt đầu hợp đồng hoặc payday
        double basicSalary = 0;
        if (contract.SalaryType == SalaryType.Gross)
        {
            basicSalary = (double)contract.BasicSalary;
        }
        else
        {
            basicSalary = await ExchangeFromNetToGross(user, defaultConfig, taxIncome, exchange, regional, insuranceConfig, tempNow, shiftConfig, contract);
        }
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

        double PersonalDeduction = 0;
        double DependanceDeduction = 0;

        
        DateTime lastPay = now.AddMonths(-1); //này bắt đầu tisng lương = lastPay + 1 ngày;
        var toDate = contract.EndDate.Value.Date;
        var listPayday = await _mediator.Send(new GetListPaydayRequest { });
        var lastPayday = listPayday.OrderByDescending(x => x.PaymentDay).FirstOrDefault();
        if (lastPayday != null)
        {
            if (now.Date <= lastPayday.PaymentDay.Date || now.Date <= contract.StartDate.Value.Date)
            {
                throw new Exception("Ngày tính lương không thể trùng với ngày trả lương lần trước hoặc ngày bắt đầu hợp đồng");
            }
        }
        var listAnnualDay = await _mediator.Send(new GetListAnnualByDayToDayRequest { FromDate = lastPay.Date, ToDate = toDate });
        double defaultWorkingHour = 0;
        double totalWorkingHour = 0;
        double OTHour = 0;
        double LeaveHour = 0;
        double CofieOT = 0;


        var listAttendance = await _mediator.Send(new GetListAttendanceByUserNoVm { UserId = user.Id });
        var finalList = listAttendance.Where(x => x.Day.Date >= lastPay.Date && x.Day.Date <= toDate).ToList();

        //tính có đủ ngày trong tháng ko để nộp bảo hiểm.
        var workingDays = CountDay(lastPay.Date, toDate);
        var dayinMonths = DateTime.DaysInMonth(lastPay.Year, lastPay.Month);
        if (workingDays == dayinMonths)
        {
            if (contract.InsuranceType == InsuranceType.BaseOnMinimum) //dựa trên mức tối thiểu
            {
                salaryTax = regional.Amount; // mức lương đóng bh = mức lương tối thiểu của vùng;
            }
            else if (contract.InsuranceType == InsuranceType.BaseOnOtherAmount)
            {
                salaryTax = (double)contract.InsuranceAmount; // mức lương đóng bh = mức đóng bh trong hợp đồng;
            }
            else
            {
                if (defaultConfig.BaseSalary * defaultConfig.InsuranceLimit < basicSalary)
                {
                    salaryTax = defaultConfig.BaseSalary * defaultConfig.InsuranceLimit; // mức lương đóng bh = mức đóng bh tối đa;
                }
                else
                {
                    salaryTax = (double)basicSalary;
                }
            }
        }


        foreach (var item in listAnnualDay)
        {
            var morningAttendance = finalList.Where(x => x.Day.Date == item.Day.Date && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault();
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
                CofieOT = CofieOT + TotalOTWage(item, morningAttendance, affternoonAttendance);
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
                CofieOT = CofieOT + TotalOTWage(item, morningAttendance, affternoonAttendance);

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
                CofieOT = CofieOT + TotalOTWage(item, morningAttendance, affternoonAttendance);

            }
            else if (item.ShiftType == ShiftType.NotWork)
            {
                if (morningAttendance != null && morningAttendance.OverTime != null)
                {
                    OTHour = (double)(OTHour + morningAttendance.OverTime);
                }
                if (affternoonAttendance != null && affternoonAttendance.OverTime != null)
                {
                    OTHour = (double)(OTHour + affternoonAttendance.OverTime);
                }
                CofieOT = CofieOT + TotalOTWage(item, morningAttendance, affternoonAttendance);

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
        double salaryPerHour = (double)(basicSalary / defaultWorkingHour);
        double totalSalary = Math.Round(salaryPerHour * finalHour);
        double leaveDeduction = (double)(basicSalary - totalSalary);
        if (leaveDeduction <= 1)
        {
            leaveDeduction = 0;
        }
        LeaveHour = defaultWorkingHour - totalWorkingHour;
        if (LeaveHour == 0)
        {
            leaveDeduction = 0;
        }
        //tính bảo hiểm:
        BHXH_Emp_Amount = Math.Round((salaryTax * insuranceConfig.BHXH_Emp / 100));
        BHYT_Emp_Amount = Math.Round((salaryTax * insuranceConfig.BHYT_Emp / 100));
        BHTN_Emp_Amount = Math.Round((salaryTax * insuranceConfig.BHTN_Emp / 100));

        var totalBH_Emp = BHYT_Emp_Amount + BHTN_Emp_Amount + BHXH_Emp_Amount;

        //thu nhập trước thuế = 
        var TNTT = basicSalary - totalBH_Emp - leaveDeduction; ///tính lại lương

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
            numOfDependance = listDependance.Where(x => x.AcceptanceType == AcceptanceType.Accept).Count();
        }

        if (numOfDependance > 0)
        {
            DependanceDeduction = Math.Round(defaultConfig.DependentTaxDeduction * numOfDependance);
        }

        // thu nhập chịu thuế = thu nhập trước thuế - giảm trừ gia cảnh bản thân và giảm trừ người phụ thuộc
        var TNCT = (double)TNTT - PersonalDeduction - DependanceDeduction;
        if (TNCT < 0)
        {
            TNCT = 0;
        }
        DetailTaxIncome tax = new DetailTaxIncome();
        if (TNCT == 0)
        {
            tax = taxIncome.OrderBy(x => x.Thue_suat).FirstOrDefault();
        }
        else
        {
            var listTax = taxIncome.Where(x => x.Muc_chiu_thue_From < TNCT).ToList();
            var temp = taxIncome.Where(x => x.Muc_chiu_thue_From < TNCT).ToList();
            if (listTax.Count() == taxIncome.Count())
            {
                tax = taxIncome.LastOrDefault();
            }
            else
            {
                tax = taxIncome.Where(x => x.Muc_chiu_thue_From < TNCT && x.Muc_chiu_thue_To >= TNCT).ToList().FirstOrDefault();
            }

        }

        // double TotalTaxIncome =Math.Round( (TNCT * tax.Thue_suat/100) - tax.He_so_tru);
        double TotalTaxIncome = 0;

        List<DetailTax> DetailTaxs = new List<DetailTax>();
        foreach (var item in taxIncome.OrderBy(x => x.Thue_suat))
        {
            var taxDetail = new DetailTax();
            if (item.Thue_suat < tax.Thue_suat)
            {
                taxDetail.Muc_chiu_thue_From = item.Muc_chiu_thue_From;
                taxDetail.Muc_chiu_thue_To = item.Muc_chiu_thue_To;
                taxDetail.Thue_suat = item.Thue_suat;
                if (taxDetail.Muc_chiu_thue_To != null)
                {
                    taxDetail.TaxAmount = Math.Round((double)((item.Muc_chiu_thue_To - item.Muc_chiu_thue_From) * item.Thue_suat / 100));
                }
            }
            else if (item.Thue_suat > tax.Thue_suat)
            {
                taxDetail.Muc_chiu_thue_From = item.Muc_chiu_thue_From;
                taxDetail.Muc_chiu_thue_To = item.Muc_chiu_thue_To;
                taxDetail.Thue_suat = item.Thue_suat;
                taxDetail.TaxAmount = 0;
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
        double totalAllowance = 0;
        if (listAllowance != null)
        {
            foreach (var item in listAllowance)
            {
                totalAllowance = totalAllowance + item.Allowance.Amount;
            }
        }
        double totalDepartmentAllowance = 0;

        var departmentAllowance = await _mediator.Send(new GetDepartmentAllowanceByDepartmentIdRequest { Id = user.Position.DepartmentId });
        if (departmentAllowance != null)
        {
            foreach (var item in departmentAllowance)
            {
                totalDepartmentAllowance = totalDepartmentAllowance + item.Subsidize.Amount;
            }
        }
        /*        double OTWage = Math.Round(OTHour * salaryPerHour );*/
        double TNST = TNCT - TotalTaxIncome;
        double OTwage = Math.Round(CofieOT * salaryPerHour);
        double netSalary = (double)(TNTT - TotalTaxIncome + totalAllowance + totalDepartmentAllowance + OTwage);

        //tính bảo hiểm:
        BHXH_Comp_Amount = Math.Round((salaryTax * insuranceConfig.BHXH_Comp / 100));
        BHYT_Comp_Amount = Math.Round((salaryTax * insuranceConfig.BHYT_Comp / 100));
        BHTN_Comp_Amount = Math.Round((salaryTax * insuranceConfig.BHTN_Comp / 100));

        var totalBH_Comp = BHXH_Comp_Amount + BHYT_Comp_Amount + BHTN_Comp_Amount;

        PayslipViewModel payslip = new PayslipViewModel();
        payslip.EmployeeContractId = contract.Id;
        payslip.Code = contract.ContractCode;
        payslip.FromTime = lastPay.Date;
        payslip.ToTime = yesterday.Date;
        payslip.PaydayCal = now;
        payslip.SalaryPerHour = salaryPerHour;
        payslip.Standard_Work_Hours = defaultWorkingHour;
        payslip.Actual_Work_Hours = totalWorkingHour;
        payslip.Ot_Hours = OTHour;
        payslip.Leave_Hours = LeaveHour;
        payslip.DefaultSalary = basicSalary;
        payslip.SalaryType = contract.SalaryType;
        payslip.InsuranceType = contract.InsuranceType;
        payslip.InsuranceAmount = salaryTax;
        payslip.isPersonalTaxDeduction = contract.isPersonalTaxDeduction;
        payslip.PersonalTaxDeductionAmount = PersonalDeduction;
        payslip.DependentTaxDeductionAmount = DependanceDeduction;
        payslip.RegionType = regional.RegionType;
        payslip.RegionMinimumWage = regional.Amount;
        payslip.IsMaternity = user.IsMaternity;
        payslip.MaternityHour = MaternityHour;

        payslip.BHXH_Emp_Amount = BHXH_Emp_Amount;
        payslip.BHYT_Emp_Amount = BHYT_Emp_Amount;
        payslip.BHTN_Emp_Amount = BHTN_Emp_Amount;
        payslip.BHXH_Emp_Percent = insuranceConfig.BHXH_Emp;
        payslip.BHYT_Emp_Percent = insuranceConfig.BHYT_Emp;
        payslip.BHTN_Emp_Percent = insuranceConfig.BHTN_Emp;

        payslip.BHXH_Comp_Amount = BHXH_Comp_Amount;
        payslip.BHYT_Comp_Amount = BHYT_Comp_Amount;
        payslip.BHTN_Comp_Amount = BHTN_Comp_Amount;
        payslip.BHXH_Comp_Percent = insuranceConfig.BHXH_Comp;
        payslip.BHYT_Comp_Percent = insuranceConfig.BHYT_Comp;
        payslip.BHTN_Comp_Percent = insuranceConfig.BHTN_Comp;

        payslip.TotalInsuranceEmp = totalBH_Emp;
        payslip.TotalInsuranceComp = totalBH_Comp;

        payslip.LeaveWageDeduction = leaveDeduction;
        payslip.TaxableSalary = TNCT;
        payslip.NumberOfDependent = numOfDependance;
        payslip.TotalDependentAmount = DependanceDeduction;
        payslip.TNTT = TNTT;
        payslip.TotalTaxIncome = TotalTaxIncome;

        payslip.AfterTaxSalary = TNST;
        payslip.TotalDepartmentAllowance = totalDepartmentAllowance;
        payslip.TotalContractAllowance = totalAllowance;

        payslip.OTWage = OTwage;
        payslip.FinalSalary = netSalary;
        payslip.Note = "Bảng lương nhân viên từ ngày " + lastPay.Date.ToString("dd/MM/yyy") + " đến ngày " + yesterday.ToString("dd/MM/yyyy") + ".";

        payslip.BankName = user.BankName;
        payslip.BankAcountName = user.BankAccountName;
        payslip.BankAcountNumber = user.BankAccountNumber;

        try
        {
            var payslipId = await _mediator.Send(new CreatePayslipCommand { payslip = payslip });
            foreach (var item in DetailTaxs)
            {
                DetailTax detail = new DetailTax();
                detail.PayslipId = payslipId;
                detail.Muc_chiu_thue_From = item.Muc_chiu_thue_From;
                detail.Muc_chiu_thue_To = item.Muc_chiu_thue_To;
                detail.Thue_suat = item.Thue_suat;
                detail.TaxAmount = item.TaxAmount;
                await _mediator.Send(new CreateDetailTaxCommand { DetailTax = detail });
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return "Tạo thành công";

    }
}