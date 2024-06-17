using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socialize.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class PropertiesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lastname",
                schema: "Domain",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Domain",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lastname",
                schema: "Domain",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Domain",
                table: "Users");
        }
    }
}
