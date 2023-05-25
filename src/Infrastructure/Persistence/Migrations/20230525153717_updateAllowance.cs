using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mentorv1.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateAllowance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Allowance_EmployeeContract_EmployeeId",
                table: "Allowance");

            migrationBuilder.DropIndex(
                name: "IX_Allowance_EmployeeId",
                table: "Allowance");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Allowance");

            migrationBuilder.CreateTable(
                name: "AllowanceEmployee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AllowanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowanceEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllowanceEmployee_Allowance_AllowanceId",
                        column: x => x.AllowanceId,
                        principalTable: "Allowance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllowanceEmployee_EmployeeContract_EmployeeContractId",
                        column: x => x.EmployeeContractId,
                        principalTable: "EmployeeContract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllowanceEmployee_AllowanceId",
                table: "AllowanceEmployee",
                column: "AllowanceId");

            migrationBuilder.CreateIndex(
                name: "IX_AllowanceEmployee_EmployeeContractId",
                table: "AllowanceEmployee",
                column: "EmployeeContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllowanceEmployee");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Allowance",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Allowance_EmployeeId",
                table: "Allowance",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Allowance_EmployeeContract_EmployeeId",
                table: "Allowance",
                column: "EmployeeId",
                principalTable: "EmployeeContract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
