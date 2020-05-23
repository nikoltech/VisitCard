using Microsoft.EntityFrameworkCore.Migrations;

namespace VisitCardApp.Migrations
{
    public partial class UpdateFileInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlPath",
                table: "ProjectCases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlPath",
                table: "ArticleImages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlPath",
                table: "ProjectCases");

            migrationBuilder.DropColumn(
                name: "UrlPath",
                table: "ArticleImages");
        }
    }
}
