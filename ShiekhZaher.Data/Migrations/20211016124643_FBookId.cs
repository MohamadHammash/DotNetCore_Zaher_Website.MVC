using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Zaher.Data.Migrations
{
    public partial class FBookId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_FBooks_FBookId",
                table: "Sections");

            migrationBuilder.AlterColumn<Guid>(
                name: "FBookId",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_FBooks_FBookId",
                table: "Sections",
                column: "FBookId",
                principalTable: "FBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_FBooks_FBookId",
                table: "Sections");

            migrationBuilder.AlterColumn<Guid>(
                name: "FBookId",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_FBooks_FBookId",
                table: "Sections",
                column: "FBookId",
                principalTable: "FBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
