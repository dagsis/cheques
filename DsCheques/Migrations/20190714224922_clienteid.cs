using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DsCheques.Migrations
{
    public partial class clienteid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdCliente",
                table: "Cheques",
                newName: "ClienteId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaIngreso",
                table: "Cheques",
                type: "smalldatetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaDeposito",
                table: "Cheques",
                type: "smalldatetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cheques_ClienteId",
                table: "Cheques",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheques_Clientes_ClienteId",
                table: "Cheques",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheques_Clientes_ClienteId",
                table: "Cheques");

            migrationBuilder.DropIndex(
                name: "IX_Cheques_ClienteId",
                table: "Cheques");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "Cheques",
                newName: "IdCliente");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaIngreso",
                table: "Cheques",
                type: "smalldatetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaDeposito",
                table: "Cheques",
                type: "smalldatetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime");
        }
    }
}
