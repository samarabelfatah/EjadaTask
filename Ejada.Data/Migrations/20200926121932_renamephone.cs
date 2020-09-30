using Microsoft.EntityFrameworkCore.Migrations;

namespace Ejada.Data.Migrations
{
    public partial class renamephone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Employee");

            migrationBuilder.AddColumn<int>(
                name: "Mobile",
                table: "Employee",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Employee");

            migrationBuilder.AddColumn<int>(
                name: "Phone",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
