using Microsoft.EntityFrameworkCore.Migrations;

namespace VisitCardApp.Migrations
{
    public partial class UpdateProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "ProjectCases");

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeType",
                table: "ProjectCases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeType",
                table: "ArticleImages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageMimeType",
                table: "ProjectCases");

            migrationBuilder.DropColumn(
                name: "ImageMimeType",
                table: "ArticleImages");

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "ProjectCases",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
