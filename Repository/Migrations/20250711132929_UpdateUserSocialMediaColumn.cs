using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSocialMediaColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaLinks_AspNetUsers_AppUserId",
                table: "SocialMediaLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialMediaLinks",
                table: "SocialMediaLinks");

            migrationBuilder.DropIndex(
                name: "IX_SocialMediaLinks_AppUserId",
                table: "SocialMediaLinks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SocialMediaLinks");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "SocialMediaLinks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialMediaLinks",
                table: "SocialMediaLinks",
                columns: new[] { "AppUserId", "SocialMediaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaLinks_AspNetUsers_AppUserId",
                table: "SocialMediaLinks",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaLinks_AspNetUsers_AppUserId",
                table: "SocialMediaLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialMediaLinks",
                table: "SocialMediaLinks");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "SocialMediaLinks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SocialMediaLinks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialMediaLinks",
                table: "SocialMediaLinks",
                columns: new[] { "UserId", "SocialMediaId" });

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaLinks_AppUserId",
                table: "SocialMediaLinks",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaLinks_AspNetUsers_AppUserId",
                table: "SocialMediaLinks",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
