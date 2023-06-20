using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace mentorv1.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Muc_Quy_Doi",
                table: "Exchanges",
                newName: "Muc_Quy_Doi_To");

            migrationBuilder.RenameColumn(
                name: "Muc_chiu_thue",
                table: "DetailTaxIncomes",
                newName: "Muc_chiu_thue_From");

            migrationBuilder.AlterColumn<double>(
                name: "Thue_Suat",
                table: "Exchanges",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Giam_Tru",
                table: "Exchanges",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Muc_Quy_Doi_From",
                table: "Exchanges",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "He_so_tru",
                table: "DetailTaxIncomes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Muc_chiu_thue_To",
                table: "DetailTaxIncomes",
                type: "float",
                nullable: true);

            migrationBuilder.InsertData(
                table: "DetailTaxIncomes",
                columns: new[] { "Id", "Created", "CreatedBy", "He_so_tru", "IsDeleted", "LastModified", "LastModifiedBy", "Muc_chiu_thue_From", "Muc_chiu_thue_To", "Thue_suat" },
                values: new object[,]
                {
                    { new Guid("029fe2a4-4e0c-4008-91c4-03c2753955d5"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 3250000.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 32000000.0, 52000000.0, 25.0 },
                    { new Guid("29052a9d-fb7c-42ee-b7c1-dbc159da4069"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 5850000.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 52000000.0, 80000000.0, 30.0 },
                    { new Guid("3334c3d2-a2cb-41b2-a9ce-10bc284456d1"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 250000.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 5000000.0, 10000000.0, 10.0 },
                    { new Guid("66196c62-d126-44df-b94a-a59515e16ce4"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1650000.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 18000000.0, 32000000.0, 20.0 },
                    { new Guid("6e64f928-88c8-4b11-8833-108a3246ab61"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0.0, 5000000.0, 5.0 },
                    { new Guid("c08eeb41-0955-4f71-a5dc-ec2359a565f3"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 9850000.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 80000000.0, null, 35.0 },
                    { new Guid("d54ee76c-e2d3-44f9-9006-784e481b881e"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 750000.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 10000000.0, 18000000.0, 15.0 }
                });

            migrationBuilder.InsertData(
                table: "Exchanges",
                columns: new[] { "Id", "Created", "CreatedBy", "Giam_Tru", "IsDeleted", "LastModified", "LastModifiedBy", "Muc_Quy_Doi_From", "Muc_Quy_Doi_To", "Thue_Suat" },
                values: new object[,]
                {
                    { new Guid("084daab6-9d5d-46b5-8cf9-305b62587610"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0.0, 4750000.0, 0.94999999999999996 },
                    { new Guid("317b9f1b-738b-4c46-b3c2-2861f3db8f2f"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 750000.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 9250000.0, 16050000.0, 0.84999999999999998 },
                    { new Guid("3b415c3e-d3de-4869-8d09-2f78ce4490c1"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1650000.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 16050000.0, 27250000.0, 0.80000000000000004 },
                    { new Guid("3cae8d06-9386-47f1-ba90-ece556ac66e1"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 250000.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 4750000.0, 9250000.0, 0.90000000000000002 },
                    { new Guid("42021ad4-5806-42fd-a665-0faba74611c4"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 3250000.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 27250000.0, 42250000.0, 0.75 },
                    { new Guid("9776d56f-20a8-4054-badf-eb7605d70aef"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 5850000.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 42250000.0, 61850000.0, 0.69999999999999996 },
                    { new Guid("9cbde439-bfef-4bf7-9d88-714bfd559cd8"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 9850000.0, false, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 61850000.0, null, 0.65000000000000002 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("029fe2a4-4e0c-4008-91c4-03c2753955d5"));

            migrationBuilder.DeleteData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("29052a9d-fb7c-42ee-b7c1-dbc159da4069"));

            migrationBuilder.DeleteData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("3334c3d2-a2cb-41b2-a9ce-10bc284456d1"));

            migrationBuilder.DeleteData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("66196c62-d126-44df-b94a-a59515e16ce4"));

            migrationBuilder.DeleteData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("6e64f928-88c8-4b11-8833-108a3246ab61"));

            migrationBuilder.DeleteData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("c08eeb41-0955-4f71-a5dc-ec2359a565f3"));

            migrationBuilder.DeleteData(
                table: "DetailTaxIncomes",
                keyColumn: "Id",
                keyValue: new Guid("d54ee76c-e2d3-44f9-9006-784e481b881e"));

            migrationBuilder.DeleteData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("084daab6-9d5d-46b5-8cf9-305b62587610"));

            migrationBuilder.DeleteData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("317b9f1b-738b-4c46-b3c2-2861f3db8f2f"));

            migrationBuilder.DeleteData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("3b415c3e-d3de-4869-8d09-2f78ce4490c1"));

            migrationBuilder.DeleteData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("3cae8d06-9386-47f1-ba90-ece556ac66e1"));

            migrationBuilder.DeleteData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("42021ad4-5806-42fd-a665-0faba74611c4"));

            migrationBuilder.DeleteData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("9776d56f-20a8-4054-badf-eb7605d70aef"));

            migrationBuilder.DeleteData(
                table: "Exchanges",
                keyColumn: "Id",
                keyValue: new Guid("9cbde439-bfef-4bf7-9d88-714bfd559cd8"));

            migrationBuilder.DropColumn(
                name: "Muc_Quy_Doi_From",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "He_so_tru",
                table: "DetailTaxIncomes");

            migrationBuilder.DropColumn(
                name: "Muc_chiu_thue_To",
                table: "DetailTaxIncomes");

            migrationBuilder.RenameColumn(
                name: "Muc_Quy_Doi_To",
                table: "Exchanges",
                newName: "Muc_Quy_Doi");

            migrationBuilder.RenameColumn(
                name: "Muc_chiu_thue_From",
                table: "DetailTaxIncomes",
                newName: "Muc_chiu_thue");

            migrationBuilder.AlterColumn<double>(
                name: "Thue_Suat",
                table: "Exchanges",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Giam_Tru",
                table: "Exchanges",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
