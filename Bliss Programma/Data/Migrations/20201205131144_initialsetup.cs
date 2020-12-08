using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bliss_Programma.Data.Migrations
{
    public partial class initialsetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locatie",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Straatnaam = table.Column<string>(nullable: true),
                    Nummer = table.Column<int>(nullable: false),
                    Toevoeging = table.Column<string>(nullable: true),
                    Postcode = table.Column<string>(nullable: true),
                    Plaatsnaam = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locatie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ruimte",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lengte = table.Column<int>(nullable: false),
                    Breedte = table.Column<int>(nullable: false),
                    Oppervlakte = table.Column<int>(nullable: false),
                    MaxWerkplekken = table.Column<int>(nullable: false),
                    Naam = table.Column<string>(nullable: true),
                    LocatieId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ruimte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ruimte_Locatie_LocatieId",
                        column: x => x.LocatieId,
                        principalTable: "Locatie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservering",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateTime>(nullable: false),
                    WerknemerId = table.Column<string>(maxLength: 450, nullable: false),
                    RuimteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservering", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservering_AspNetUsers_WerknemerId",
                        column: x => x.WerknemerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservering_Ruimte_RuimteId",
                        column: x => x.RuimteId,
                        principalTable: "Ruimte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservering_RuimteId",
                table: "Reservering",
                column: "RuimteId");

            migrationBuilder.CreateIndex(
                name: "IX_Ruimte_LocatieId",
                table: "Ruimte",
                column: "LocatieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservering");

            migrationBuilder.DropTable(
                name: "Ruimte");

            migrationBuilder.DropTable(
                name: "Locatie");
        }
    }
}
