using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zaher.Data.Migrations
{
    public partial class ToBeCheckedSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "Description", "FBookId", "Title" },
                values: new object[] { new Guid("cb231d1c-5d5b-4ccd-ad0c-9713d4c460e5"), "يحتوي على الأسئلة التي على أحد المشرفين مراجعتها أو حذفها", new Guid("b303404b-881c-4bda-95e2-5207dcc2d60e"), "الأسئلة المشبوهة" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("cb231d1c-5d5b-4ccd-ad0c-9713d4c460e5"));
        }
    }
}
