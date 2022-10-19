using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Niue.Migrations
{
    public partial class RenameDueDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Jobs",
                newName: "DueDateUTC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DueDateUTC",
                table: "Jobs",
                newName: "DueDate");
        }
    }
}
