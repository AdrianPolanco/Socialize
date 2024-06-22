using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socialize.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class ProfilePhotoUrlAddedToDomainUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                schema: "Domain",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                schema: "Domain",
                table: "Users");
        }
    }
}
