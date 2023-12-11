using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EastonPartners.Infrastructure.Persistence.Migrations
{
    public partial class AddedAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Partner",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Partner",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Partner",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Partner",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Partner",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Partner");
        }
    }
}
