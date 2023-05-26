using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mentorv1.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Degree_Employee_EmployeeId",
                table: "Degree");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeContract_Employee_EmployeeId",
                table: "EmployeeContract");

            migrationBuilder.DropForeignKey(
                name: "FK_Experience_AspNetUsers_UserId",
                table: "Experience");

            migrationBuilder.DropForeignKey(
                name: "FK_Experience_Employee_EmployeeId",
                table: "Experience");

            migrationBuilder.DropForeignKey(
                name: "FK_InterviewProcess_Employee_EmployeeId",
                table: "InterviewProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveLog_Employee_EmployeeId",
                table: "LeaveLog");

            migrationBuilder.DropForeignKey(
                name: "FK_OvertimeLog_Employee_EmployeeId",
                table: "OvertimeLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Position_Employee_EmployeeId",
                table: "Position");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestChange_Employee_EmployeeId",
                table: "RequestChange");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillEmployee_Employee_EmployeeId",
                table: "SkillEmployee");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_SkillEmployee_EmployeeId",
                table: "SkillEmployee");

            migrationBuilder.DropIndex(
                name: "IX_RequestChange_EmployeeId",
                table: "RequestChange");

            migrationBuilder.DropIndex(
                name: "IX_Position_EmployeeId",
                table: "Position");

            migrationBuilder.DropIndex(
                name: "IX_OvertimeLog_EmployeeId",
                table: "OvertimeLog");

            migrationBuilder.DropIndex(
                name: "IX_LeaveLog_EmployeeId",
                table: "LeaveLog");

            migrationBuilder.DropIndex(
                name: "IX_InterviewProcess_EmployeeId",
                table: "InterviewProcess");

            migrationBuilder.DropIndex(
                name: "IX_Experience_EmployeeId",
                table: "Experience");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeContract_EmployeeId",
                table: "EmployeeContract");

            migrationBuilder.DropIndex(
                name: "IX_Degree_EmployeeId",
                table: "Degree");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "SkillEmployee");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "RequestChange");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Position");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "OvertimeLog");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "LeaveLog");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "InterviewProcess");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "EmployeeContract");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Degree");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Experience",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Experience_UserId",
                table: "Experience",
                newName: "IX_Experience_ApplicationUserId");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "SkillEmployee",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "RequestChange",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountName",
                table: "RequestChange",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "RequestChange",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "RequestChange",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDay",
                table: "RequestChange",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "RequestChange",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fullname",
                table: "RequestChange",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "RequestChange",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "RequestChange",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "RequestChange",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Position",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "OvertimeLog",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "LeaveLog",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "InterviewProcess",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "EmployeeContract",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Degree",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankAccountName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDay",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fullname",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SkillEmployee_ApplicationUserId",
                table: "SkillEmployee",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Position_ApplicationUserId",
                table: "Position",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OvertimeLog_ApplicationUserId",
                table: "OvertimeLog",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveLog_ApplicationUserId",
                table: "LeaveLog",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewProcess_ApplicationUserId",
                table: "InterviewProcess",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContract_ApplicationUserId",
                table: "EmployeeContract",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Degree_ApplicationUserId",
                table: "Degree",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Degree_AspNetUsers_ApplicationUserId",
                table: "Degree",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeContract_AspNetUsers_ApplicationUserId",
                table: "EmployeeContract",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_AspNetUsers_ApplicationUserId",
                table: "Experience",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewProcess_AspNetUsers_ApplicationUserId",
                table: "InterviewProcess",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveLog_AspNetUsers_ApplicationUserId",
                table: "LeaveLog",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OvertimeLog_AspNetUsers_ApplicationUserId",
                table: "OvertimeLog",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Position_AspNetUsers_ApplicationUserId",
                table: "Position",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillEmployee_AspNetUsers_ApplicationUserId",
                table: "SkillEmployee",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Degree_AspNetUsers_ApplicationUserId",
                table: "Degree");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeContract_AspNetUsers_ApplicationUserId",
                table: "EmployeeContract");

            migrationBuilder.DropForeignKey(
                name: "FK_Experience_AspNetUsers_ApplicationUserId",
                table: "Experience");

            migrationBuilder.DropForeignKey(
                name: "FK_InterviewProcess_AspNetUsers_ApplicationUserId",
                table: "InterviewProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveLog_AspNetUsers_ApplicationUserId",
                table: "LeaveLog");

            migrationBuilder.DropForeignKey(
                name: "FK_OvertimeLog_AspNetUsers_ApplicationUserId",
                table: "OvertimeLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Position_AspNetUsers_ApplicationUserId",
                table: "Position");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillEmployee_AspNetUsers_ApplicationUserId",
                table: "SkillEmployee");

            migrationBuilder.DropIndex(
                name: "IX_SkillEmployee_ApplicationUserId",
                table: "SkillEmployee");

            migrationBuilder.DropIndex(
                name: "IX_Position_ApplicationUserId",
                table: "Position");

            migrationBuilder.DropIndex(
                name: "IX_OvertimeLog_ApplicationUserId",
                table: "OvertimeLog");

            migrationBuilder.DropIndex(
                name: "IX_LeaveLog_ApplicationUserId",
                table: "LeaveLog");

            migrationBuilder.DropIndex(
                name: "IX_InterviewProcess_ApplicationUserId",
                table: "InterviewProcess");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeContract_ApplicationUserId",
                table: "EmployeeContract");

            migrationBuilder.DropIndex(
                name: "IX_Degree_ApplicationUserId",
                table: "Degree");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "SkillEmployee");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "RequestChange");

            migrationBuilder.DropColumn(
                name: "BankAccountName",
                table: "RequestChange");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "RequestChange");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "RequestChange");

            migrationBuilder.DropColumn(
                name: "BirthDay",
                table: "RequestChange");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "RequestChange");

            migrationBuilder.DropColumn(
                name: "Fullname",
                table: "RequestChange");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "RequestChange");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "RequestChange");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "RequestChange");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Position");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "OvertimeLog");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "LeaveLog");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "InterviewProcess");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "EmployeeContract");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Degree");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BankAccountName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BirthDay",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Fullname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Experience",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Experience_ApplicationUserId",
                table: "Experience",
                newName: "IX_Experience_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "SkillEmployee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "RequestChange",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Position",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "OvertimeLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "LeaveLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "InterviewProcess",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Experience",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "EmployeeContract",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Degree",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentityNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillEmployee_EmployeeId",
                table: "SkillEmployee",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestChange_EmployeeId",
                table: "RequestChange",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Position_EmployeeId",
                table: "Position",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OvertimeLog_EmployeeId",
                table: "OvertimeLog",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveLog_EmployeeId",
                table: "LeaveLog",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewProcess_EmployeeId",
                table: "InterviewProcess",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Experience_EmployeeId",
                table: "Experience",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContract_EmployeeId",
                table: "EmployeeContract",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Degree_EmployeeId",
                table: "Degree",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ApplicationUserId",
                table: "Employee",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employee",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Degree_Employee_EmployeeId",
                table: "Degree",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeContract_Employee_EmployeeId",
                table: "EmployeeContract",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_AspNetUsers_UserId",
                table: "Experience",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_Employee_EmployeeId",
                table: "Experience",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewProcess_Employee_EmployeeId",
                table: "InterviewProcess",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveLog_Employee_EmployeeId",
                table: "LeaveLog",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OvertimeLog_Employee_EmployeeId",
                table: "OvertimeLog",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Position_Employee_EmployeeId",
                table: "Position",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestChange_Employee_EmployeeId",
                table: "RequestChange",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillEmployee_Employee_EmployeeId",
                table: "SkillEmployee",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
