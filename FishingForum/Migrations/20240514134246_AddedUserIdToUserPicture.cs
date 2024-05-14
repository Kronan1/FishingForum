using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FishingForum.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserIdToUserPicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "UserPicture",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserPicture",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserPicture_CreatedById",
                table: "UserPicture",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPicture_AspNetUsers_CreatedById",
                table: "UserPicture",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPicture_AspNetUsers_CreatedById",
                table: "UserPicture");

            migrationBuilder.DropIndex(
                name: "IX_UserPicture_CreatedById",
                table: "UserPicture");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "UserPicture");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserPicture");
        }
    }
}
