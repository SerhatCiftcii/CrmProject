using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class newmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizedPeople_AspNetUsers_AppUserId",
                table: "AuthorizedPeople");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AuthorizedPeople",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AuthorizedPeople",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AuthorizedPeople",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizedPeople_AspNetUsers_AppUserId",
                table: "AuthorizedPeople",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizedPeople_AspNetUsers_AppUserId",
                table: "AuthorizedPeople");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "AuthorizedPeople");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AuthorizedPeople");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AuthorizedPeople");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizedPeople_AspNetUsers_AppUserId",
                table: "AuthorizedPeople",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
