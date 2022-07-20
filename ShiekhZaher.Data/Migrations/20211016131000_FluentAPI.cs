using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Zaher.Data.Migrations
{
    public partial class FluentAPI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FBooks",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[] { new Guid("b303404b-881c-4bda-95e2-5207dcc2d60e"), "يحتوي على الأسئلة التي لم يتم تصنيفها بعد", "غير مصنف" });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "Description", "FBookId", "Title" },
                values: new object[] { new Guid("85761b0c-d3f5-4456-b178-b5bcb07e6078"), "يحتوي على الأسئلة التي لم يتم تصنيفها بعد", new Guid("b303404b-881c-4bda-95e2-5207dcc2d60e"), "غير مصنف" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("85761b0c-d3f5-4456-b178-b5bcb07e6078"));

            migrationBuilder.DeleteData(
                table: "FBooks",
                keyColumn: "Id",
                keyValue: new Guid("b303404b-881c-4bda-95e2-5207dcc2d60e"));
        }
    }
}
