﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socialize.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class EmailUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "Domain",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                schema: "Domain",
                table: "Users");
        }
    }
}
