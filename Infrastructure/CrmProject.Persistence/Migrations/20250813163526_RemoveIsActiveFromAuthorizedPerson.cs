using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsActiveFromAuthorizedPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AuthorizedPeople");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AuthorizedPeople",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
