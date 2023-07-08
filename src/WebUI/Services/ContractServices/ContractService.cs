using System.Diagnostics.Contracts;
using System.Threading;
using FluentValidation;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.DefaultConfig.Queries.Get;
using mentor_v1.Application.EmployeeContract.Commands.CreateEmpContract;
using mentor_v1.Application.EmployeeContract.Commands.DeleteEmpContract;
using mentor_v1.Application.EmployeeContract.Commands.UpdateEmpContract;
using mentor_v1.Application.RegionalMinimumWage.Queries;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using WebUI.Services.Format;

namespace WebUI.Services.ContractServices;

public class ContractService : IContracService
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFormatMoney _format;
    private readonly IMediator _mediator;

    public ContractService(IApplicationDbContext context, UserManager<ApplicationUser> userManager, IFormatMoney format,IMediator mediator)
    {
        _context = context;
        _userManager = userManager;
        _format = format;
        _mediator = mediator;
    }
    public async Task<List<string>> CheckValidatorCreate(CreateEmployeeContractCommand model)
    {
        List<string> errors = new List<string>();
        ApplicationUser user;
        try
        {
            user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                var item = "Không tìm thấy người dùng bạn yêu cầu!";
                errors.Add(item);
                return errors;
            }
        }
        catch (Exception)
        {
            var item = "Không tìm thấy người dùng bạn yêu cầu!";
            errors.Add(item);
            return errors;
        }
        if(user.WorkStatus== WorkStatus.Quit)
        {
            var item = "Hiện " + user.Fullname + " - " + user.UserName + " không còn làm việc tại TechGenius. Vì vậy không thể thêm hợp đồng nữa!";
            errors.Add(item);
            return errors;
        }
        var listwait = _context.Get<EmployeeContract>().Where(x => x.ApplicationUserId == user.Id && x.IsDeleted == false && x.Status == EmployeeContractStatus.Waiting).ToList();
        if (listwait != null && listwait.Count > 0)
        {

            string end = listwait.FirstOrDefault().EndDate.Value.ToString("dd/MM/yyyy");
            var item = "Hiện " + user.Fullname + " - " + user.UserName + " đang có hợp đồng đang đợi bắt đầu. Vì vậy không thể thêm hợp đồng nữa!";
            errors.Add(item);
            return errors;
        }

        var openContract = _context.Get<EmployeeContract>().Where(x => x.ApplicationUserId == user.Id && x.IsDeleted == false && x.Status == EmployeeContractStatus.Pending && x.ContractType == ContractType.OpenEnded).FirstOrDefault();
        if (openContract != null)
        {

            var item = "Hiện " + user.Fullname + " - " + user.UserName + " đang có hợp đồng dài hạn. Vì vậy không thể thêm hợp đồng nữa!";
            errors.Add(item);
            return errors;
        }



        //check validation:
        var validator = new CreateEmpContractValidator(_context);
        var valResult = await validator.ValidateAsync(model);

        if (valResult.Errors.Count != 0)
        {
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage; errors.Add(item);
            }

        }
        if (model.ContractType == ContractType.FixedTerm && model.EndDate == null)
        {
            var item = "Với loại hợp đồng có thời hạn phải có ngày kết thúc hợp đồng!";
            errors.Add(item);
        }
        else if (model.ContractType == ContractType.FixedTerm)
        {
            if (model.EndDate > model.StartDate.Value.AddMonths(36) || model.EndDate < model.StartDate.Value.AddMonths(12))
            {
                var item = "Hợp đồng lao động có thời hạn có chỉ thời hạn từ 12 - 36 tháng.";
                errors.Add(item);
            }
        }
        else if (model.ContractType == ContractType.FixedTerm && model.EndDate != null && model.EndDate < model.StartDate)
        {
            var item = "Với loại hợp đồng có thời hạn phải có ngày kết thúc phải lớn hơn ngày bắt đầu hợp đồng!";
            errors.Add(item);
        }

        if (model.InsuranceType == InsuranceType.BaseOnOtherAmount && model.InsuranceAmount <= 0)
        {
            var item = "Với hình thức đóng bảo hiểm dựa trên số khác yêu cầu phải cung cấp mức đóng bảo hiểm!";
            errors.Add(item);
        }
        var defaultConfig = await _mediator.Send(new GetDefaultConfigRequest { });
        if(model.InsuranceType == InsuranceType.BaseOnOtherAmount && model.InsuranceAmount> defaultConfig.InsuranceLimit * defaultConfig.BaseSalary)
        {
            var item = "Với hình thức đóng bảo hiểm dựa trên số khác, Mức đóng bảo hiểm không quá "+defaultConfig.InsuranceLimit+ " lần mức lương cơ sở (Mức lương cơ sở: "+defaultConfig.BaseSalary+" VNĐ)!";
            errors.Add(item);
        }
        var listExp = _context.Get<EmployeeContract>().Where(x => x.ApplicationUserId == user.Id && x.IsDeleted == false && x.ContractType == ContractType.FixedTerm).ToList();
        if (listExp != null && listExp.Count >= 2 && model.ContractType == ContractType.FixedTerm)
        {
            var item = "Hiện " + user.Fullname + " - " + user.UserName + " đã có tối đa 2 hợp đồng có thời hạn theo quy định của Pháp luật. Vui lòng chuyển loại hợp đồng thành hợp đồng vô thời hạn!";
            errors.Add(item);
        }
        try
        {
            var config = await _mediator.Send(new GetDefaultConfigRequest { });
            var region = await _mediator.Send(new GetRegionalWageByRegionTypeNoVm { RegionType = config.CompanyRegionType });
            if (model.BasicSalary < region.Amount)
            {
                string amount = _format.Format(region.Amount);
                var item = "Lương cơ bản không được thấp hơn mức lương tối thiểu của vùng (" + region.RegionType.ToString() + ": " + amount + " VND)";
                errors.Add(item);
            }
        }


        catch (Exception)
        {
            List<string> newItem = new List<string>();
            var item = "Đã xảy ra lỗi.Vui lòng kiểm tra lại các cấu hình về lương và vùng!";
            newItem.Add(item);
            return newItem;
        }
        return errors;
    }

    public async Task<List<string>> CheckValidatorCreateEmployee(CreateEmployeeContractCommand model)
    {


        List<string> errors = new List<string>();
        
        //check validation:
        var validator = new CreateEmpContractValidator(_context);
        var valResult = await validator.ValidateAsync(model);

        if (valResult.Errors.Count != 0)
        {
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage; errors.Add(item);
            }

        }
        if (model.ContractType == ContractType.FixedTerm && model.EndDate == null)
        {
            var item = "Với loại hợp đồng có thời hạn phải có ngày kết thúc hợp đồng!";
            errors.Add(item);
        }
        else if (model.ContractType == ContractType.FixedTerm)
        {
            if (model.EndDate > model.StartDate.Value.AddMonths(36) || model.EndDate < model.StartDate.Value.AddMonths(12))
            {
                var item = "Hợp đồng lao động có thời hạn có chỉ thời hạn từ 12 - 36 tháng.";
                errors.Add(item);
            }
        }
        else if (model.ContractType == ContractType.FixedTerm && model.EndDate != null && model.EndDate < model.StartDate)
        {
            var item = "Với loại hợp đồng có thời hạn phải có ngày kết thúc phải lớn hơn ngày bắt đầu hợp đồng!";
            errors.Add(item);
        }

        if (model.InsuranceType == InsuranceType.BaseOnOtherAmount && model.InsuranceAmount <= 0)
        {
            var item = "Với hình thức đóng bảo hiểm dựa trên số khác yêu cầu phải cung cấp mức đóng bảo hiểm!";
            errors.Add(item);
        }

        var defaultConfig = await _mediator.Send(new GetDefaultConfigRequest { });
        if (model.InsuranceType == InsuranceType.BaseOnOtherAmount && model.InsuranceAmount > defaultConfig.InsuranceLimit * defaultConfig.BaseSalary)
        {
            var item = "Với hình thức đóng bảo hiểm dựa trên số khác, Mức đóng bảo hiểm không quá " + defaultConfig.InsuranceLimit + " lần mức lương cơ sở (Mức lương cơ sở: " + defaultConfig.BaseSalary + " VNĐ)!";
            errors.Add(item);
        }
        try
        {
            var config = await _mediator.Send(new GetDefaultConfigRequest { });
            var region = await _mediator.Send(new GetRegionalWageByRegionTypeNoVm { RegionType = config.CompanyRegionType });
            if (model.BasicSalary < region.Amount)
            {
                string amount = _format.Format(region.Amount);
                var item = "Lương cơ bản không được thấp hơn mức lương tối thiểu của vùng (" + region.RegionType.ToString() + ": " + amount + " VND)";
                errors.Add(item);
            }
        }
        catch (Exception)
        {
            List<string> newItem = new List<string>();
            var item = "Đã xảy ra lỗi.Vui lòng kiểm tra lại các cấu hình về lương và vùng!";
            newItem.Add(item);
            return newItem;
        }

        if (model.AllowanceId != null && model.AllowanceId.Length > 0)
        {
            foreach (var item in model.AllowanceId)
            {
                var allowance = _context.Get<mentor_v1.Domain.Entities.Allowance>().Where(x => x.Id == item).FirstOrDefault();
                if (allowance == null)
                {
                    var temp = "Danh sách trợ cấp theo hợp đồng không hợp lệ!";
                    errors.Add(temp);
                }
            }
        }
        return errors;
    }

    public async Task<List<string>> CheckValidatorUpdate(UpdateEmpContractCommand model)
    {
        List<string> errors = new List<string>();
        ApplicationUser user;
        var contract = _context.Get<EmployeeContract>().Where(x => x.Id == model.Id && x.IsDeleted == false).FirstOrDefault();
        if(contract.Status == EmployeeContractStatus.Expeires )
        {
            var item = "Không thể cập nhật đối với các hợp đồng đã hết hạn!";
            errors.Add(item);
            return errors;
        }

        if (contract == null)
        {
            var item = "Không tìm thấy hợp đồng bạn yêu cầu!";
            errors.Add(item);
            return errors;
            
        }
        
            user = await _userManager.FindByIdAsync(contract.ApplicationUserId);
            if (user == null)
            {
                var item = "Không tìm thấy người dùng bạn yêu cầu!";
                errors.Add(item);
                return errors;
            }
        
        //check validation:
        var validator = new UpdateEmpContractCommandValidator(_context);
        var valResult = await validator.ValidateAsync(model);

        if (valResult.Errors.Count != 0)
        {
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage; errors.Add(item);
            }

        }
        if (model.ContractType == ContractType.FixedTerm && model.EndDate == null)
        {
            var item = "Với loại hợp đồng có thời hạn phải có ngày kết thúc hợp đồng!";
            errors.Add(item);
        }
        else if (model.ContractType == ContractType.FixedTerm)
        {
            if (model.EndDate > model.StartDate.Value.AddMonths(36) || model.EndDate < model.StartDate.Value.AddMonths(12))
            {
                var item = "Hợp đồng lao động có thời hạn có chỉ thời hạn từ 12 - 36 tháng.";
                errors.Add(item);
            }
        }
        else if (model.ContractType == ContractType.FixedTerm && model.EndDate != null && model.EndDate < model.StartDate)
        {
            var item = "Với loại hợp đồng có thời hạn phải có ngày kết thúc phải lớn hơn ngày bắt đầu hợp đồng!";
            errors.Add(item);
        }

        if (model.InsuranceType == InsuranceType.BaseOnOtherAmount && model.InsuranceAmount <= 0)
        {
            var item = "Với hình thức đóng bảo hiểm dựa trên số khác yêu cầu phải cung cấp mức đóng bảo hiểm!";
            errors.Add(item);
        }
        var defaultConfig = await _mediator.Send(new GetDefaultConfigRequest { });
        if (model.InsuranceType == InsuranceType.BaseOnOtherAmount && model.InsuranceAmount > defaultConfig.InsuranceLimit * defaultConfig.BaseSalary)
        {
            var item = "Với hình thức đóng bảo hiểm dựa trên số khác, Mức đóng bảo hiểm không quá " + defaultConfig.InsuranceLimit + " lần mức lương cơ sở (Mức lương cơ sở: " + defaultConfig.BaseSalary + " VNĐ)!";
            errors.Add(item);
        }
        var listExp = _context.Get<EmployeeContract>().Where(x => x.Id != contract.Id && x.ApplicationUserId == user.Id  && x.IsDeleted == false && x.ContractType == ContractType.FixedTerm).ToList();
        if (listExp != null && listExp.Count >= 2 && model.ContractType == ContractType.FixedTerm)
        {
            var item = "Hiện " + user.Fullname + " - " + user.UserName + " đã có tối đa 2 hợp đồng có thời hạn theo quy định của Pháp luật!";
            errors.Add(item);
        }
        try
        {
            var config = await _mediator.Send(new GetDefaultConfigRequest { });
            var region = await _mediator.Send(new GetRegionalWageByRegionTypeNoVm { RegionType = config.CompanyRegionType });
            if (model.BasicSalary < region.Amount)
            {
                string amount = _format.Format(region.Amount);
                var item = "Lương cơ bản không được thấp hơn mức lương tối thiểu của vùng (" + region.RegionType.ToString() + ": " + amount + " VND)";
                errors.Add(item);
            }
        }
        catch (Exception)
        {
            List<string> newItem = new List<string>();
            var item = "Đã xảy ra lỗi.Vui lòng kiểm tra lại các cấu hình về lương và vùng!";
            newItem.Add(item);
            return newItem;
        }
        return errors;
    }
}
