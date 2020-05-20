using Microsoft.EntityFrameworkCore.Migrations;

namespace VisitCardApp.Migrations
{
    public partial class ProjectArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleImage_Articles_ArticleId",
                table: "ArticleImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleImage",
                table: "ArticleImage");

            migrationBuilder.RenameTable(
                name: "ArticleImage",
                newName: "ArticleImages");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleImage_ArticleId",
                table: "ArticleImages",
                newName: "IX_ArticleImages_ArticleId");

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleImages",
                table: "ArticleImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleImages_Articles_ArticleId",
                table: "ArticleImages",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleImages_Articles_ArticleId",
                table: "ArticleImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleImages",
                table: "ArticleImages");

            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Articles");

            migrationBuilder.RenameTable(
                name: "ArticleImages",
                newName: "ArticleImage");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleImages_ArticleId",
                table: "ArticleImage",
                newName: "IX_ArticleImage_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleImage",
                table: "ArticleImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleImage_Articles_ArticleId",
                table: "ArticleImage",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
