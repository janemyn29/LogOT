using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace mentorv1.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateConfigDay2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CoefficientId",
                table: "AnnualWorkingDays",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ConfigDayId",
                table: "AnnualWorkingDays",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Coefficients",
                columns: new[] { "Id", "AmountCoefficient", "Created", "CreatedBy", "IsDeleted", "LastModified", "LastModifiedBy", "TypeDate" },
                values: new object[,]
                {
                    { new Guid("22104ebc-c6e6-4f44-a7b6-344752e8d1e5"), 1.5, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", 3 },
                    { new Guid("7fd46536-291c-40f0-8f19-0aeed5d26e63"), 1.5, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", 3 },
                    { new Guid("a510ba38-65d8-445c-95fd-f1b719b19c08"), 1.0, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", 1 },
                    { new Guid("b861adcd-208c-4b6c-bef1-962cd147a6f7"), 1.5, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", 2 }
                });

            migrationBuilder.InsertData(
                table: "ConfigDays",
                columns: new[] { "Id", "Created", "CreatedBy", "Holiday", "IsDeleted", "LastModified", "LastModifiedBy", "Normal", "Saturday", "Sunday" },
                values: new object[] { new Guid("ea7cebd4-6de8-40a3-958b-f4d28ee9c843"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", 8, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", 1, 3, 8 });

            migrationBuilder.CreateIndex(
                name: "IX_AnnualWorkingDays_CoefficientId",
                table: "AnnualWorkingDays",
                column: "CoefficientId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualWorkingDays_Coefficients_CoefficientId",
                table: "AnnualWorkingDays");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualWorkingDays_ConfigDays_ConfigDayId",
                table: "AnnualWorkingDays");

            migrationBuilder.DropIndex(
                name: "IX_AnnualWorkingDays_CoefficientId",
                table: "AnnualWorkingDays");

            migrationBuilder.DropIndex(
                name: "IX_AnnualWorkingDays_ConfigDayId",
                table: "AnnualWorkingDays");

            migrationBuilder.DeleteData(
                table: "Coefficients",
                keyColumn: "Id",
                keyValue: new Guid("22104ebc-c6e6-4f44-a7b6-344752e8d1e5"));

            migrationBuilder.DeleteData(
                table: "Coefficients",
                keyColumn: "Id",
                keyValue: new Guid("7fd46536-291c-40f0-8f19-0aeed5d26e63"));

            migrationBuilder.DeleteData(
                table: "Coefficients",
                keyColumn: "Id",
                keyValue: new Guid("a510ba38-65d8-445c-95fd-f1b719b19c08"));

            migrationBuilder.DeleteData(
                table: "Coefficients",
                keyColumn: "Id",
                keyValue: new Guid("b861adcd-208c-4b6c-bef1-962cd147a6f7"));

            migrationBuilder.DeleteData(
                table: "ConfigDays",
                keyColumn: "Id",
                keyValue: new Guid("ea7cebd4-6de8-40a3-958b-f4d28ee9c843"));

            migrationBuilder.DropColumn(
                name: "CoefficientId",
                table: "AnnualWorkingDays");

            migrationBuilder.DropColumn(
                name: "ConfigDayId",
                table: "AnnualWorkingDays");
        }
    }
}
