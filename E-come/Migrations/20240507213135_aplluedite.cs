using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_come.Migrations
{
    /// <inheritdoc />
    public partial class aplluedite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "applyShops",
                newName: "TheDoorNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TheDoorNumber",
                table: "applyShops",
                newName: "Name");
        }
    }
}
