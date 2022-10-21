using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Niue.Migrations
{
    public partial class LogOutputRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Record",
                table: "Logs",
                newName: "Output");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Output",
                table: "Logs",
                newName: "Record");
        }
    }
}
