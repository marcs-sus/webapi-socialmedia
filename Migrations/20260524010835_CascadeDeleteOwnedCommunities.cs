using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiSocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteOwnedCommunities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Users_OwnerId",
                table: "Communities");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Users_OwnerId",
                table: "Communities",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Users_OwnerId",
                table: "Communities");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Users_OwnerId",
                table: "Communities",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
