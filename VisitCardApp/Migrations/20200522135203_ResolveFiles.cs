using Microsoft.EntityFrameworkCore.Migrations;

namespace VisitCardApp.Migrations
{
    public partial class ResolveFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "ProjectCases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "ProjectCases");
        }
    }
}
