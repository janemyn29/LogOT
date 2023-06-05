using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mentorv1.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateBase64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageBase",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBase",
                table: "AspNetUsers");
        }
    }
}
