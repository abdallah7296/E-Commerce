﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_come.Migrations
{
    /// <inheritdoc />
    public partial class shopuserrrr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "shopsProducts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_shopsProducts_UserId",
                table: "shopsProducts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_shopsProducts_AspNetUsers_UserId",
                table: "shopsProducts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shopsProducts_AspNetUsers_UserId",
                table: "shopsProducts");

            migrationBuilder.DropIndex(
                name: "IX_shopsProducts_UserId",
                table: "shopsProducts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "shopsProducts");
        }
    }
}
