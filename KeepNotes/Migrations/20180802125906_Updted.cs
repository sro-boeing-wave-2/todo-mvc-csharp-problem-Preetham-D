using Microsoft.EntityFrameworkCore.Migrations;

namespace KeepNotes.Migrations
{
    public partial class Updted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "list",
                table: "CheckList",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "list",
                table: "CheckList");
        }
    }
}
