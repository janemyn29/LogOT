using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mentorv1.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class payslip3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TNTT",
                table: "PaySlip",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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

            migrationBuilder.UpdateData(
                table: "InsuranceConfigs",
                keyColumn: "Id",
                keyValue: new Guid("38f2c77a-b5da-484b-a836-3befc8fb9b89"),
                columns: new[] { "BHCD_Comp", "BHTN_Emp", "BHXH_Comp", "BHXH_Emp", "BHYT_Comp", "BHYT_Emp" },
                values: new object[] { 2.0, 1.0, 17.5, 8.0, 3.0, 1.5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TNTT",
                table: "PaySlip");

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
                table: "InsuranceConfigs",
                keyColumn: "Id",
                keyValue: new Guid("38f2c77a-b5da-484b-a836-3befc8fb9b89"),
                columns: new[] { "BHCD_Comp", "BHTN_Emp", "BHXH_Comp", "BHXH_Emp", "BHYT_Comp", "BHYT_Emp" },
                values: new object[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
        }
    }
}
