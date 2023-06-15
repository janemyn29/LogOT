using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mentorv1.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateCoefficient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualWorkingDays_Coefficients_CoefficientId",
                table: "AnnualWorkingDays");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualWorkingDays_ConfigDays_ConfigDayId",
                table: "AnnualWorkingDays");

            migrationBuilder.DropIndex(
                name: "IX_AnnualWorkingDays_ConfigDayId",
                table: "AnnualWorkingDays");

            migrationBuilder.DropColumn(
                name: "ConfigDayId",
                table: "AnnualWorkingDays");

            migrationBuilder.AlterColumn<Guid>(
                name: "CoefficientId",
                table: "AnnualWorkingDays",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualWorkingDays_Coefficients_CoefficientId",
                table: "AnnualWorkingDays",
                column: "CoefficientId",
                principalTable: "Coefficients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualWorkingDays_Coefficients_CoefficientId",
                table: "AnnualWorkingDays");

            migrationBuilder.AlterColumn<Guid>(
                name: "CoefficientId",
                table: "AnnualWorkingDays",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ConfigDayId",
                table: "AnnualWorkingDays",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AnnualWorkingDays_ConfigDayId",
                table: "AnnualWorkingDays",
                column: "ConfigDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualWorkingDays_Coefficients_CoefficientId",
                table: "AnnualWorkingDays",
                column: "CoefficientId",
                principalTable: "Coefficients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualWorkingDays_ConfigDays_ConfigDayId",
                table: "AnnualWorkingDays",
                column: "ConfigDayId",
                principalTable: "ConfigDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
