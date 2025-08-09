using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorizedPersonAppUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "AuthorizedPeople",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizedPeople_AppUserId",
                table: "AuthorizedPeople",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizedPeople_AspNetUsers_AppUserId",
                table: "AuthorizedPeople",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizedPeople_AspNetUsers_AppUserId",
                table: "AuthorizedPeople");

            migrationBuilder.DropIndex(
                name: "IX_AuthorizedPeople_AppUserId",
                table: "AuthorizedPeople");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AuthorizedPeople");
        }
    }
}
