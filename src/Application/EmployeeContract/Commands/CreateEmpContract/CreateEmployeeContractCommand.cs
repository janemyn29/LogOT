﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.EmployeeContract.Commands.CreateEmpContract;
public class CreateEmployeeContractCommand : IRequest<Guid>
{
    public string Username { get; set; }
    public string ContractCode { get; set; }
    public string? File { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Job { get; set; }
    public double? BasicSalary { get; set; }
    public EmployeeContractStatus Status { get; set; }
    public double? PercentDeduction { get; set; }
    public SalaryType SalaryType { get; set; }
    public ContractType ContractType { get; set; }
    public bool isPersonalTaxDeduction { get; set; }
    public InsuranceType InsuranceType { get; set; }
    public double? InsuranceAmount { get; set; }
    public Guid[]? AllowanceId { get; set; }
}

public class CreateEmployeeContractCommandHandler : IRequestHandler<CreateEmployeeContractCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;


    public CreateEmployeeContractCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Guid> Handle(CreateEmployeeContractCommand request, CancellationToken cancellationToken)
    {
        mentor_v1.Domain.Identity.ApplicationUser user;
        try
        {
            user = await _userManager.FindByNameAsync(request.Username);
            if(user==null)
            {
                throw new NotFoundException("Không tìm thấy người dùng bạn yêu cầu!");
            }
        }
        catch (Exception)
        {

            throw new NotFoundException("Không tìm thấy người dùng bạn yêu cầu!");
        }
        
        var city = new Domain.Entities.EmployeeContract()
        {
            ApplicationUserId =user.Id,
            File = request.File,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Job = request.Job,
            BasicSalary = request.BasicSalary,
            Status = request.Status,
            PercentDeduction = request.PercentDeduction,
            SalaryType = request.SalaryType,
            ContractType = request.ContractType,
            ContractCode = request.ContractCode,
            InsuranceAmount = request.InsuranceAmount,
            InsuranceType = request.InsuranceType,
            isPersonalTaxDeduction = request.isPersonalTaxDeduction,
        };

        // add new category
        _context.Get<Domain.Entities.EmployeeContract>().Add(city);

        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);

        if (request.AllowanceId!=null&& request.AllowanceId.Length > 0)
        {
            foreach (var item in request.AllowanceId)
            {
                var allowance = _context.Get<Domain.Entities.Allowance>().Where(x => x.Id == item).FirstOrDefault();
                if(allowance==null) {
                    throw new Exception("Danh sách trợ cấp theo hợp đồng không hợp lệ!");
                }
            }
            foreach (var item in request.AllowanceId.Distinct())
            {
                var allowance = new Domain.Entities.AllowanceEmployee
                {
                    AllowanceId = item,
                    EmployeeContractId = city.Id,
                };
                // add new category
                _context.Get<Domain.Entities.AllowanceEmployee>().Add(allowance);

                // commit change to database
                // because the function is async so we await it
                await _context.SaveChangesAsync(cancellationToken);

            }
        }
            // return the Guid
            return city.Id;
    }
}
