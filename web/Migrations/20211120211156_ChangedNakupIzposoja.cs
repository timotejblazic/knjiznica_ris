using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web.Migrations
{
    public partial class ChangedNakupIzposoja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GradivoIzvod_IzposojaID",
                table: "GradivoIzvod");

            migrationBuilder.DropIndex(
                name: "IX_GradivoIzvod_NakupID",
                table: "GradivoIzvod");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cena",
                table: "Nakup",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "IdKupljenegaGradiva",
                table: "Nakup",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdIzposojenegaGradiva",
                table: "Izposoja",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GradivoIzvod_IzposojaID",
                table: "GradivoIzvod",
                column: "IzposojaID",
                unique: true,
                filter: "[IzposojaID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GradivoIzvod_NakupID",
                table: "GradivoIzvod",
                column: "NakupID",
                unique: true,
                filter: "[NakupID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GradivoIzvod_IzposojaID",
                table: "GradivoIzvod");

            migrationBuilder.DropIndex(
                name: "IX_GradivoIzvod_NakupID",
                table: "GradivoIzvod");

            migrationBuilder.DropColumn(
                name: "IdKupljenegaGradiva",
                table: "Nakup");

            migrationBuilder.DropColumn(
                name: "IdIzposojenegaGradiva",
                table: "Izposoja");

            migrationBuilder.AlterColumn<string>(
                name: "Cena",
                table: "Nakup",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_GradivoIzvod_IzposojaID",
                table: "GradivoIzvod",
                column: "IzposojaID");

            migrationBuilder.CreateIndex(
                name: "IX_GradivoIzvod_NakupID",
                table: "GradivoIzvod",
                column: "NakupID");
        }
    }
}
