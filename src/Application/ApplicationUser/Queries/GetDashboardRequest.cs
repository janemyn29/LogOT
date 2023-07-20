using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.PagingUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace mentor_v1.Application.ApplicationUser.Queries;
public class GetDashboardRequest : IRequest<List<Dashboard>>
{
}

// IRequestHandler<request type, return type>
public class GetDashboardRequestHandler : IRequestHandler<GetDashboardRequest, List<Dashboard>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManage;

    // DI
    public GetDashboardRequestHandler(IApplicationDbContext context, IMapper mapper, UserManager<Domain.Identity.ApplicationUser> userManage)
    {
        _context = context;
        _mapper = mapper;
        _userManage = userManage;
    }

    public async Task<List<Dashboard>> Handle(GetDashboardRequest request, CancellationToken cancellationToken)
    {
        List<Dashboard> model = new List<Dashboard>();
        //số nhân viên đang làm
        var listEmp = await _userManage.GetUsersInRoleAsync("Employee");
        var numEmp = listEmp.Where(x=>x.WorkStatus == Domain.Enums.WorkStatus.StillWork).Count();
        Dashboard employeeModel = new Dashboard("Tổng nhân viên đang làm", numEmp.ToString());
        model.Add(employeeModel);

        //Tổng báo cáo tự động:
        var totalJob = await _context.Get<Domain.Entities.JobReport>().Where(x => x.IsDeleted == false).CountAsync();
        Dashboard totalJobModel = new Dashboard("Tổng số báo cáo tự động", totalJob.ToString());
        model.Add(totalJobModel);

        //Tổng phòng ban:
        var totalDepartment = await _context.Get<Domain.Entities.Department>().Where(x => x.IsDeleted == false).CountAsync();
        Dashboard totalDepartmentModel = new Dashboard("Tổng số phòng ban", totalDepartment.ToString());
        model.Add(totalDepartmentModel);


        //tổng hợp đồng:
        var totalContract = await _context.Get<Domain.Entities.EmployeeContract>().Where(x => x.IsDeleted == false && x.Status == Domain.Enums.EmployeeContractStatus.Pending).CountAsync();
        Dashboard totalContractModel = new Dashboard("Tổng số hợp đồng đang diễn ra", totalDepartment.ToString());
        model.Add(totalContractModel);


        //số tổng lương
        var totalSalary = await _context.Get<Domain.Entities.PaySlip>().Where(x => x.IsDeleted == false).SumAsync(x => x.FinalSalary);
        Dashboard totalSalaryModel = new Dashboard("Tổng số lương", Format(totalSalary.Value) + "VNĐ");
        model.Add(totalSalaryModel);


        int Month = DateTime.Now.Month-1;
        int Year = DateTime.Now.Year;

        if (Month == 0)
        {
            Year = Year - 1;
            Month = 12;
        }




        //tổng lương đã trả tháng trước:
        var totalSalaryMonth = await _context.Get<Domain.Entities.PaySlip>().Where(x => x.IsDeleted == false && x.ToTime.Date.Month == Month && x.ToTime.Year == Year).SumAsync(x => x.FinalSalary);
        Dashboard totalSalaryMonthModel = new Dashboard("Tổng số lương tháng trước", Format(totalSalaryMonth.Value) + "VNĐ");
        model.Add(totalSalaryMonthModel);

        //tổng bảo hiểm tháng
        double InsMOnth = 0;
        var totalBHMonth = await _context.Get<Domain.Entities.PaySlip>().Where(x => x.IsDeleted == false && x.PaydayCal.AddDays(-1).Date.Month == Month && x.PaydayCal.AddDays(-1).Year == Year).ToListAsync();
        if(totalBHMonth!=null&& totalBHMonth.Count > 0)
        {
            foreach (var item in totalBHMonth)
            {
                InsMOnth = InsMOnth + item.BHTN_Comp_Amount + item.BHXH_Comp_Amount + item.BHYT_Comp_Amount + item.TotalInsuranceEmp;
            }
        }
        Dashboard totalBHMonthModel = new Dashboard("Tổng bảo hiểm tháng trước", Format(InsMOnth) + "VNĐ");
        model.Add(totalBHMonthModel);

        //tổng bảo hiểm
        double Ins = 0;
        var totalBH = await _context.Get<Domain.Entities.PaySlip>().Where(x => x.IsDeleted == false).ToListAsync();
        if (totalBH != null && totalBH.Count > 0)
        {
            foreach (var item in totalBH)
            {
                Ins = Ins+ item.BHTN_Comp_Amount+ item.BHXH_Comp_Amount+ item.BHYT_Comp_Amount + item.TotalInsuranceEmp;
            }
        }
        Dashboard totalBHModel = new Dashboard("Tổng bảo hiểm đã nộp", Format(Ins) + "VNĐ");
        model.Add(totalBHModel);




        //tổng Thuế tháng
        double taxMonth = 0;
        var totalTaxMonth = await _context.Get<Domain.Entities.PaySlip>().Where(x => x.IsDeleted == false && x.PaydayCal.AddDays(-1).Date.Month == Month && x.PaydayCal.AddDays(-1).Year == Year).ToListAsync();
        if (totalTaxMonth != null && totalTaxMonth.Count > 0)
        {
            foreach (var item in totalTaxMonth)
            {
                taxMonth = taxMonth + item.TotalTaxIncome;
            }
        }
        Dashboard finaltaxMonth = new Dashboard("Tổng Thuế thu nhập tháng trước", Format(taxMonth) + "VNĐ");
        model.Add(finaltaxMonth);

        //tổng bảo hiểm
        double Tax = 0;
        var totalTax = await _context.Get<Domain.Entities.PaySlip>().Where(x => x.IsDeleted == false).ToListAsync();
        if (totalTax != null && totalTax.Count > 0)
        {
            foreach (var item in totalTax)
            {
                Tax = Tax +item.TotalTaxIncome;
            }
        }
        Dashboard totalTaxIncome = new Dashboard("Tổng Thuế thu nhập", Format(Tax) + "VNĐ");
        model.Add(totalTaxIncome);



        return model;
    }

    public string Format(double value)
    {
        string strNumber = value.ToString();

        string reversedStrNumber = ReverseString(strNumber);
        string formattedNumber = InsertDots(reversedStrNumber, 3, '.');

        string finalNumber = ReverseString(formattedNumber);
        return finalNumber;
    }

    public string ReverseString(string str)
    {
        char[] charArray = str.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public string InsertDots(string str, int interval, char dotChar)
    {
        int length = str.Length;
        int dotCount = (int)Math.Ceiling((double)length / interval) - 1;
        char[] dottedArray = new char[length + dotCount];

        int index = 0;
        for (int i = 0; i < length; i++)
        {
            dottedArray[index++] = str[i];
            if ((i + 1) % interval == 0 && i != length - 1)
            {
                dottedArray[index++] = dotChar;
            }
        }

        return new string(dottedArray);
    }
}
