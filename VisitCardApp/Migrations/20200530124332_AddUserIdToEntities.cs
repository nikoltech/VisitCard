using Microsoft.EntityFrameworkCore.Migrations;

namespace VisitCardApp.Migrations
{
    public partial class AddUserIdToEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "Comments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Articles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UserId",
                table: "Articles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_UserId",
                table: "Articles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Articles_ArticleId",
                table: "Comments",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_UserId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Articles_ArticleId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Articles_UserId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Articles");
        }
    }
}
