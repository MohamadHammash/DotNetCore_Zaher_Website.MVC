using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Zaher.Data.Migrations
{
    public partial class SectionsFbooksAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SectionId",
                table: "QAs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FBooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FBookId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_FBooks_FBookId",
                        column: x => x.FBookId,
                        principalTable: "FBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QAs_SectionId",
                table: "QAs",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_FBookId",
                table: "Sections",
                column: "FBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_QAs_Sections_SectionId",
                table: "QAs",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QAs_Sections_SectionId",
                table: "QAs");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "FBooks");

            migrationBuilder.DropIndex(
                name: "IX_QAs_SectionId",
                table: "QAs");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "QAs");
        }
    }
}
