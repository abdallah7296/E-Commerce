using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_come.Migrations
{
    /// <inheritdoc />
    public partial class addusername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "applyShops",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_applyShops_UserName",
                table: "applyShops",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_applyShops_AspNetUsers_UserName",
                table: "applyShops",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applyShops_AspNetUsers_UserName",
                table: "applyShops");

            migrationBuilder.DropIndex(
                name: "IX_applyShops_UserName",
                table: "applyShops");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "applyShops");
        }
    }
}
