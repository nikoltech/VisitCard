using Microsoft.EntityFrameworkCore.Migrations;

namespace VisitCardApp.Migrations
{
    public partial class UpdateFileFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "ArticleImages");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "ArticleImages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "ArticleImages");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "ArticleImages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
