using Microsoft.EntityFrameworkCore.Migrations;

namespace Bliss_Programma.Data.Migrations
{
    public partial class WerknemerEmailAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WerknemerEmail",
                table: "Reservering",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WerknemerEmail",
                table: "Reservering");
        }
    }
}
