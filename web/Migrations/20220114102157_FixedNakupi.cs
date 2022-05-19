using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web.Migrations
{
    public partial class FixedNakupi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cena",
                table: "Nakup");

            migrationBuilder.AlterColumn<decimal>(
                name: "IdKupljenegaGradiva",
                table: "Nakup",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdKupljenegaGradiva",
                table: "Nakup",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "Cena",
                table: "Nakup",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
