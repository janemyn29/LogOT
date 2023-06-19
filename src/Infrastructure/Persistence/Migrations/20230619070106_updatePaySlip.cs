using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mentorv1.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatePaySlip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaternityEmployee_MaternityAllowance_MaternityAllowanceId",
                table: "MaternityEmployee");

            migrationBuilder.DropTable(
                name: "MaternityAllowance");

            migrationBuilder.DropIndex(
                name: "IX_MaternityEmployee_MaternityAllowanceId",
                table: "MaternityEmployee");

            migrationBuilder.DropColumn(
                name: "BHTN_Comp",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHTN_Emp",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHXH_Comp",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHXH_Emp",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHYT_Comp",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "MaternityEmployee");

            migrationBuilder.DropColumn(
                name: "MaternityAllowanceId",
                table: "MaternityEmployee");

            migrationBuilder.RenameColumn(
                name: "Total_Salary",
                table: "PaySlip",
                newName: "TotalDepartmentAllowance");

            migrationBuilder.RenameColumn(
                name: "Tax_In_Come",
                table: "PaySlip",
                newName: "TotalContractAllowance");

            migrationBuilder.RenameColumn(
                name: "Deduction",
                table: "PaySlip",
                newName: "OTWage");

            migrationBuilder.RenameColumn(
                name: "Bonus",
                table: "PaySlip",
                newName: "LeaveWageDeduction");

            migrationBuilder.RenameColumn(
                name: "Base_Salary",
                table: "PaySlip",
                newName: "FinalSalary");

            migrationBuilder.RenameColumn(
                name: "BHYT_Emp",
                table: "PaySlip",
                newName: "DefaultSalary");

            migrationBuilder.AddColumn<double>(
                name: "AfterTaxSalary",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BHTN_Comp_Amount",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BHTN_Comp_Percent",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BHTN_Emp_Amount",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BHTN_Emp_Percent",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BHXH_Comp_Amount",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BHXH_Comp_Percent",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BHXH_Emp_Amount",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BHXH_Emp_Percent",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BHYT_Comp_Amount",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BHYT_Comp_Percent",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BHYT_Emp_Amount",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BHYT_Emp_Percent",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DependentTaxDeductionAmount",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "InsuranceAmount",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "InsuranceType",
                table: "PaySlip",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsMaternity",
                table: "PaySlip",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfDependent",
                table: "PaySlip",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PersonalTaxDeductionAmount",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RegionMinimumWage",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RegionType",
                table: "PaySlip",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SalaryPerHour",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "SalaryType",
                table: "PaySlip",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TaxPercent",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TaxableSalary",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalDependentAmount",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalInsuranceComp",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalInsuranceEmp",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalTaxIncome",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "isPersonalTaxDeduction",
                table: "PaySlip",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DenyReason",
                table: "MaternityEmployee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InsuranceLimit",
                table: "DefaultConfig",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DefaultConfig",
                keyColumn: "Id",
                keyValue: new Guid("581e5321-94d3-4a13-8f95-c2938462e2fa"),
                column: "InsuranceLimit",
                value: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AfterTaxSalary",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHTN_Comp_Amount",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHTN_Comp_Percent",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHTN_Emp_Amount",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHTN_Emp_Percent",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHXH_Comp_Amount",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHXH_Comp_Percent",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHXH_Emp_Amount",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHXH_Emp_Percent",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHYT_Comp_Amount",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHYT_Comp_Percent",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHYT_Emp_Amount",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "BHYT_Emp_Percent",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "DependentTaxDeductionAmount",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "InsuranceAmount",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "InsuranceType",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "IsMaternity",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "NumberOfDependent",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "PersonalTaxDeductionAmount",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "RegionMinimumWage",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "RegionType",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "SalaryPerHour",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "SalaryType",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "TaxPercent",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "TaxableSalary",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "TotalDependentAmount",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "TotalInsuranceComp",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "TotalInsuranceEmp",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "TotalTaxIncome",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "isPersonalTaxDeduction",
                table: "PaySlip");

            migrationBuilder.DropColumn(
                name: "DenyReason",
                table: "MaternityEmployee");

            migrationBuilder.DropColumn(
                name: "InsuranceLimit",
                table: "DefaultConfig");

            migrationBuilder.RenameColumn(
                name: "TotalDepartmentAllowance",
                table: "PaySlip",
                newName: "Total_Salary");

            migrationBuilder.RenameColumn(
                name: "TotalContractAllowance",
                table: "PaySlip",
                newName: "Tax_In_Come");

            migrationBuilder.RenameColumn(
                name: "OTWage",
                table: "PaySlip",
                newName: "Deduction");

            migrationBuilder.RenameColumn(
                name: "LeaveWageDeduction",
                table: "PaySlip",
                newName: "Bonus");

            migrationBuilder.RenameColumn(
                name: "FinalSalary",
                table: "PaySlip",
                newName: "Base_Salary");

            migrationBuilder.RenameColumn(
                name: "DefaultSalary",
                table: "PaySlip",
                newName: "BHYT_Emp");

            migrationBuilder.AddColumn<double>(
                name: "BHTN_Comp",
                table: "PaySlip",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BHTN_Emp",
                table: "PaySlip",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BHXH_Comp",
                table: "PaySlip",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BHXH_Emp",
                table: "PaySlip",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BHYT_Comp",
                table: "PaySlip",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "MaternityEmployee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MaternityAllowanceId",
                table: "MaternityEmployee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "MaternityAllowance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Descrition = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaternityAllowance", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaternityEmployee_MaternityAllowanceId",
                table: "MaternityEmployee",
                column: "MaternityAllowanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaternityEmployee_MaternityAllowance_MaternityAllowanceId",
                table: "MaternityEmployee",
                column: "MaternityAllowanceId",
                principalTable: "MaternityAllowance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
