using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mentorv1.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateInsurance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsuranceConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BHXHEmp = table.Column<double>(name: "BHXH_Emp", type: "float", nullable: false),
                    BHYTEmp = table.Column<double>(name: "BHYT_Emp", type: "float", nullable: false),
                    BHTNEmp = table.Column<double>(name: "BHTN_Emp", type: "float", nullable: false),
                    BHXHComp = table.Column<double>(name: "BHXH_Comp", type: "float", nullable: false),
                    BHYTComp = table.Column<double>(name: "BHYT_Comp", type: "float", nullable: false),
                    BHTNComp = table.Column<double>(name: "BHTN_Comp", type: "float", nullable: false),
                    BHCDComp = table.Column<double>(name: "BHCD_Comp", type: "float", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceConfigs", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("029fe2a4-4e0c-4008-91c4-03c2753955d5"),
                columns: new[] { "He_so_tru", "Muc_chiu_thue_From", "Muc_chiu_thue_To", "Thue_suat" },
                values: new object[] { 3250000.0, 32000000.0, 52000000.0, 25.0 });

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("29052a9d-fb7c-42ee-b7c1-dbc159da4069"),
                columns: new[] { "He_so_tru", "Muc_chiu_thue_From", "Muc_chiu_thue_To", "Thue_suat" },
                values: new object[] { 5850000.0, 52000000.0, 80000000.0, 30.0 });

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("3334c3d2-a2cb-41b2-a9ce-10bc284456d1"),
                columns: new[] { "He_so_tru", "Muc_chiu_thue_From", "Muc_chiu_thue_To", "Thue_suat" },
                values: new object[] { 250000.0, 5000000.0, 10000000.0, 10.0 });

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("66196c62-d126-44df-b94a-a59515e16ce4"),
                columns: new[] { "He_so_tru", "Muc_chiu_thue_From", "Muc_chiu_thue_To", "Thue_suat" },
                values: new object[] { 1650000.0, 18000000.0, 32000000.0, 20.0 });

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("6e64f928-88c8-4b11-8833-108a3246ab61"),
                columns: new[] { "Muc_chiu_thue_To", "Thue_suat" },
                values: new object[] { 5000000.0, 5.0 });

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("c08eeb41-0955-4f71-a5dc-ec2359a565f3"),
                columns: new[] { "He_so_tru", "Muc_chiu_thue_From", "Thue_suat" },
                values: new object[] { 9850000.0, 80000000.0, 35.0 });

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("d54ee76c-e2d3-44f9-9006-784e481b881e"),
                columns: new[] { "He_so_tru", "Muc_chiu_thue_From", "Muc_chiu_thue_To", "Thue_suat" },
                values: new object[] { 750000.0, 10000000.0, 18000000.0, 15.0 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("084daab6-9d5d-46b5-8cf9-305b62587610"),
                columns: new[] { "Muc_Quy_Doi_To", "Thue_Suat" },
                values: new object[] { 4750000.0, 0.94999999999999996 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("317b9f1b-738b-4c46-b3c2-2861f3db8f2f"),
                columns: new[] { "Giam_Tru", "Muc_Quy_Doi_From", "Muc_Quy_Doi_To", "Thue_Suat" },
                values: new object[] { 750000.0, 9250000.0, 16050000.0, 0.84999999999999998 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("3b415c3e-d3de-4869-8d09-2f78ce4490c1"),
                columns: new[] { "Giam_Tru", "Muc_Quy_Doi_From", "Muc_Quy_Doi_To", "Thue_Suat" },
                values: new object[] { 1650000.0, 16050000.0, 27250000.0, 0.80000000000000004 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("3cae8d06-9386-47f1-ba90-ece556ac66e1"),
                columns: new[] { "Giam_Tru", "Muc_Quy_Doi_From", "Muc_Quy_Doi_To", "Thue_Suat" },
                values: new object[] { 250000.0, 4750000.0, 9250000.0, 0.90000000000000002 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("42021ad4-5806-42fd-a665-0faba74611c4"),
                columns: new[] { "Giam_Tru", "Muc_Quy_Doi_From", "Muc_Quy_Doi_To", "Thue_Suat" },
                values: new object[] { 3250000.0, 27250000.0, 42250000.0, 0.75 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("9776d56f-20a8-4054-badf-eb7605d70aef"),
                columns: new[] { "Giam_Tru", "Muc_Quy_Doi_From", "Muc_Quy_Doi_To", "Thue_Suat" },
                values: new object[] { 5850000.0, 42250000.0, 61850000.0, 0.69999999999999996 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("9cbde439-bfef-4bf7-9d88-714bfd559cd8"),
                columns: new[] { "Giam_Tru", "Muc_Quy_Doi_From", "Thue_Suat" },
                values: new object[] { 9850000.0, 61850000.0, 0.65000000000000002 });

            migrationBuilder.InsertData(
                table: "InsuranceConfigs",
                columns: new[] { "Id", "BHCD_Comp", "BHTN_Comp", "BHTN_Emp", "BHXH_Comp", "BHXH_Emp", "BHYT_Comp", "BHYT_Emp", "Created", "CreatedBy", "IsDeleted", "LastModified", "LastModifiedBy" },
                values: new object[] { new Guid("38f2c77a-b5da-484b-a836-3befc8fb9b89"), 2.0, 0.0, 1.0, 17.5, 8.0, 3.0, 1.5, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test" });

            migrationBuilder.UpdateData(
                table: "ShiftConfigs",
                keyColumn: "Id",
                keyValue: new Guid("25482772-d7ac-473b-9548-9ef38dfb1be1"),
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2023, 7, 4, 17, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 7, 4, 13, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "ShiftConfigs",
                keyColumn: "Id",
                keyValue: new Guid("8caed193-c40e-448b-bdab-cd7cd4f24844"),
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2023, 7, 4, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 7, 4, 8, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsuranceConfigs");

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("029fe2a4-4e0c-4008-91c4-03c2753955d5"),
                columns: new[] { "He_so_tru", "Muc_chiu_thue_From", "Muc_chiu_thue_To", "Thue_suat" },
                values: new object[] { 0.0, 0.0, null, 0.0 });

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("29052a9d-fb7c-42ee-b7c1-dbc159da4069"),
                columns: new[] { "He_so_tru", "Muc_chiu_thue_From", "Muc_chiu_thue_To", "Thue_suat" },
                values: new object[] { 0.0, 0.0, null, 0.0 });

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("3334c3d2-a2cb-41b2-a9ce-10bc284456d1"),
                columns: new[] { "He_so_tru", "Muc_chiu_thue_From", "Muc_chiu_thue_To", "Thue_suat" },
                values: new object[] { 0.0, 0.0, null, 0.0 });

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("66196c62-d126-44df-b94a-a59515e16ce4"),
                columns: new[] { "He_so_tru", "Muc_chiu_thue_From", "Muc_chiu_thue_To", "Thue_suat" },
                values: new object[] { 0.0, 0.0, null, 0.0 });

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("6e64f928-88c8-4b11-8833-108a3246ab61"),
                columns: new[] { "Muc_chiu_thue_To", "Thue_suat" },
                values: new object[] { null, 0.0 });

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("c08eeb41-0955-4f71-a5dc-ec2359a565f3"),
                columns: new[] { "He_so_tru", "Muc_chiu_thue_From", "Thue_suat" },
                values: new object[] { 0.0, 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("d54ee76c-e2d3-44f9-9006-784e481b881e"),
                columns: new[] { "He_so_tru", "Muc_chiu_thue_From", "Muc_chiu_thue_To", "Thue_suat" },
                values: new object[] { 0.0, 0.0, null, 0.0 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("084daab6-9d5d-46b5-8cf9-305b62587610"),
                columns: new[] { "Muc_Quy_Doi_To", "Thue_Suat" },
                values: new object[] { null, 0.0 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("317b9f1b-738b-4c46-b3c2-2861f3db8f2f"),
                columns: new[] { "Giam_Tru", "Muc_Quy_Doi_From", "Muc_Quy_Doi_To", "Thue_Suat" },
                values: new object[] { 0.0, 0.0, null, 0.0 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("3b415c3e-d3de-4869-8d09-2f78ce4490c1"),
                columns: new[] { "Giam_Tru", "Muc_Quy_Doi_From", "Muc_Quy_Doi_To", "Thue_Suat" },
                values: new object[] { 0.0, 0.0, null, 0.0 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("3cae8d06-9386-47f1-ba90-ece556ac66e1"),
                columns: new[] { "Giam_Tru", "Muc_Quy_Doi_From", "Muc_Quy_Doi_To", "Thue_Suat" },
                values: new object[] { 0.0, 0.0, null, 0.0 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("42021ad4-5806-42fd-a665-0faba74611c4"),
                columns: new[] { "Giam_Tru", "Muc_Quy_Doi_From", "Muc_Quy_Doi_To", "Thue_Suat" },
                values: new object[] { 0.0, 0.0, null, 0.0 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("9776d56f-20a8-4054-badf-eb7605d70aef"),
                columns: new[] { "Giam_Tru", "Muc_Quy_Doi_From", "Muc_Quy_Doi_To", "Thue_Suat" },
                values: new object[] { 0.0, 0.0, null, 0.0 });

            migrationBuilder.UpdateData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("9cbde439-bfef-4bf7-9d88-714bfd559cd8"),
                columns: new[] { "Giam_Tru", "Muc_Quy_Doi_From", "Thue_Suat" },
                values: new object[] { 0.0, 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "ShiftConfigs",
                keyColumn: "Id",
                keyValue: new Guid("25482772-d7ac-473b-9548-9ef38dfb1be1"),
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2023, 6, 30, 17, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 6, 30, 13, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "ShiftConfigs",
                keyColumn: "Id",
                keyValue: new Guid("8caed193-c40e-448b-bdab-cd7cd4f24844"),
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2023, 6, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 6, 30, 8, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
