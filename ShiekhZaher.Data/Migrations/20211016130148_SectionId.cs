using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Zaher.Data.Migrations
{
    public partial class SectionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QAs_Sections_SectionId",
                table: "QAs");

            migrationBuilder.AlterColumn<Guid>(
                name: "SectionId",
                table: "QAs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QAs_Sections_SectionId",
                table: "QAs",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QAs_Sections_SectionId",
                table: "QAs");

            migrationBuilder.AlterColumn<Guid>(
                name: "SectionId",
                table: "QAs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_QAs_Sections_SectionId",
                table: "QAs",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
