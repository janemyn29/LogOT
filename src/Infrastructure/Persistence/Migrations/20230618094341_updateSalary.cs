using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace mentorv1.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateSalary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "InsuranceAmount",
                table: "EmployeeContract",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InsuranceType",
                table: "EmployeeContract",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isPersonalTaxDeduction",
                table: "EmployeeContract",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DefaultConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyRegionType = table.Column<int>(type: "int", nullable: false),
                    BaseSalary = table.Column<double>(type: "float", nullable: false),
                    PersonalTaxDeduction = table.Column<double>(type: "float", nullable: false),
                    DependentTaxDeduction = table.Column<double>(type: "float", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionalMinimumWage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    RegionType = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionalMinimumWage", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DefaultConfig",
                columns: new[] { "Id", "BaseSalary", "CompanyRegionType", "Created", "CreatedBy", "DependentTaxDeduction", "IsDeleted", "LastModified", "LastModifiedBy", "PersonalTaxDeduction" },
                values: new object[] { new Guid("581e5321-94d3-4a13-8f95-c2938462e2fa"), 1490000.0, 1, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 4400000.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 11000000.0 });

            migrationBuilder.InsertData(
                table: "RegionalMinimumWage",
                columns: new[] { "Id", "Amount", "Created", "CreatedBy", "IsDeleted", "LastModified", "LastModifiedBy", "RegionType" },
                values: new object[,]
                {
                    { new Guid("2c4e8e53-7de6-4b56-a6e1-8472343677a9"), 3250000.0, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 4 },
                    { new Guid("c859f162-3e32-4f41-af77-15c4628d7f22"), 4160000.0, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 2 },
                    { new Guid("cd3262db-dd9d-409f-8944-1b8929dd9a41"), 3640000.0, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 3 },
                    { new Guid("d1564a77-716a-4e36-94f7-0f3a781548b8"), 4680000.0, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefaultConfig");

            migrationBuilder.DropTable(
                name: "RegionalMinimumWage");

            migrationBuilder.DropColumn(
                name: "InsuranceAmount",
                table: "EmployeeContract");

            migrationBuilder.DropColumn(
                name: "InsuranceType",
                table: "EmployeeContract");

            migrationBuilder.DropColumn(
                name: "isPersonalTaxDeduction",
                table: "EmployeeContract");
        }
    }
}
