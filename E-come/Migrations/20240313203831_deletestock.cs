using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_come.Migrations
{
    /// <inheritdoc />
    public partial class deletestock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stock",
                table: "products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "stock",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
