using Microsoft.EntityFrameworkCore.Migrations;

namespace Zaher.Data.Migrations
{
    public partial class EmailToQa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "QAs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "QAs");
        }
    }
}
