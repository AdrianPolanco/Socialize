using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socialize.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAttachmentIdFromPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentId",
                schema: "Domain",
                table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentId",
                schema: "Domain",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
