using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmailAndPhoneFromAuthorizedPersonn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "AuthorizedPeople");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AuthorizedPeople");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AuthorizedPeople",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AuthorizedPeople",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
