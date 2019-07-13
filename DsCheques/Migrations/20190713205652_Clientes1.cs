using Microsoft.EntityFrameworkCore.Migrations;

namespace DsCheques.Migrations
{
    public partial class Clientes1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Cuit",
                table: "Clientes",
                type: "varchar(11)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Cuit",
                table: "Clientes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(11)");
        }
    }
}
