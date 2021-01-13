using Microsoft.EntityFrameworkCore.Migrations;

namespace Bliss_Programma.Data.Migrations
{
    public partial class PrioriteitAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prioriteit",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prioriteit",
                table: "AspNetUsers");
        }
    }
}
