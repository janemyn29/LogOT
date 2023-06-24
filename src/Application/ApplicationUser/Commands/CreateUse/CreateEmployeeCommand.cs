
using System.Text.Json;
using AutoMapper;
using MediatR;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.EmployeeContract.Commands.CreateEmpContract;
using mentor_v1.Application.EmployeeContract.Commands.DeleteEmpContract;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.ApplicationUser.Commands.CreateUse;

public class CreateEmployeeCommand : IRequest<Domain.Identity.ApplicationUser>
{
   public EmployeeModel EmployeeModel { get; set; }
}

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Domain.Identity.ApplicationUser>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateEmployeeCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager,IIdentityService identityService,IMapper mapper,IMediator mediator)
    {
        _context = context;
        _userManager = userManager;
        _identityService= identityService;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Domain.Identity.ApplicationUser> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        Guid contractId;
        try
        {
            var model = _mapper.Map<UserViewModel>(request.EmployeeModel);
            var contract = _mapper.Map<CreateEmployeeContractCommand>(request.EmployeeModel);
            var result = await _identityService.CreateUserAsync(model.Username, model.Email, "Employee1!", model.Fullname, model.Image, model.Address, model.IdentityNumber, model.BirthDay, model.BankAccountNumber, model.BankAccountName, model.BankName, model.PositionId, model.GenderType, model.IsMaternity, mentor_v1.Domain.Enums.WorkStatus.StillWork,model.PhoneNumber);

            if (result.Result.Succeeded)
            {
                var user = await _identityService.FindUserByEmailAsync(model.Email);
                try
                {
                    try
                    {

                        if (contract.ContractType == ContractType.OpenEnded)
                        {
                            contract.EndDate = null;
                        }
                        var tempContract = new Domain.Entities.EmployeeContract()
                        {
                            ApplicationUserId = user.Id,
                            File = contract.File,
                            StartDate = contract.StartDate,
                            EndDate = contract.EndDate,
                            Job = contract.Job,
                            BasicSalary = contract.BasicSalary,
                            Status = EmployeeContractStatus.Waiting,
                            PercentDeduction = contract.PercentDeduction,
                            SalaryType = contract.SalaryType,
                            ContractType = contract.ContractType,
                            ContractCode = contract.ContractCode,
                            InsuranceAmount = contract.InsuranceAmount,
                            InsuranceType = contract.InsuranceType,
                            isPersonalTaxDeduction = contract.isPersonalTaxDeduction,
                        };

                        // add new category
                        _context.Get<Domain.Entities.EmployeeContract>().Add(tempContract);
                        await _context.SaveChangesAsync(cancellationToken);
                        contractId = tempContract.Id;

                        if (contract.AllowanceId != null && contract.AllowanceId.Length > 0)
                        {
                            foreach (var item in contract.AllowanceId)
                            {
                                var allowance = _context.Get<Domain.Entities.Allowance>().Where(x => x.Id == item).FirstOrDefault();
                                if (allowance == null)
                                {

                                    await _mediator.Send(new DeleteEmpContractCommand { Id = contractId });
                                    await _userManager.DeleteAsync(user);
                                    throw new Exception("Danh sách trợ cấp theo hợp đồng không hợp lệ!");
                                }
                            }
                            foreach (var item in contract.AllowanceId.Distinct())
                            {
                                var allowance = new Domain.Entities.AllowanceEmployee
                                {
                                    AllowanceId = item,
                                    EmployeeContractId = tempContract.Id,
                                };
                                _context.Get<Domain.Entities.AllowanceEmployee>().Add(allowance);
                                await _context.SaveChangesAsync(cancellationToken);

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        await _userManager.DeleteAsync(user);
                        throw new Exception("Tạo hợp đồng đã xảy ra lỗi. Vui lòng thực hiện lại!");
                    }
                    var addRoleResult = await _userManager.AddToRoleAsync(user, request.EmployeeModel.Role);
                    if (addRoleResult.Succeeded)
                    {
                        return user;
                    }
                    else
                    {
                        throw new Exception("Thêm Role bị lỗi");
                    }
                }
                catch (Exception)
                {

                    await _userManager.DeleteAsync(user);

                    throw new Exception("Thêm Role bị lỗi");
                }

            }
            else
            {
                //string jsonString = JsonSerializer.Serialize(result.Result.Errors);
                throw new Exception(result.Result.Errors.FirstOrDefault());
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
