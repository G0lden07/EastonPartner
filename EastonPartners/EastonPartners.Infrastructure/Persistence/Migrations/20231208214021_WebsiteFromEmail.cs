using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EastonPartners.Infrastructure.Persistence.Migrations
{
    public partial class WebsiteFromEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Partner",
                newName: "Website");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Website",
                table: "Partner",
                newName: "Email");
        }
    }
}
