using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_come.Migrations
{
    /// <inheritdoc />
    public partial class EditeShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "shopsProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "shopsProducts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "shopsProducts");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "shopsProducts");
        }
    }
}
