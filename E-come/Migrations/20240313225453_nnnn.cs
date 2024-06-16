using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_come.Migrations
{
    /// <inheritdoc />
    public partial class nnnn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_shopsProducts_ShopProductsId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_ShopProductsId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "ShopProductsId",
                table: "products");

            migrationBuilder.CreateIndex(
                name: "IX_products_ShopId",
                table: "products",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_shopsProducts_ShopId",
                table: "products",
                column: "ShopId",
                principalTable: "shopsProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_shopsProducts_ShopId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_ShopId",
                table: "products");

            migrationBuilder.AddColumn<int>(
                name: "ShopProductsId",
                table: "products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_ShopProductsId",
                table: "products",
                column: "ShopProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_shopsProducts_ShopProductsId",
                table: "products",
                column: "ShopProductsId",
                principalTable: "shopsProducts",
                principalColumn: "Id");
        }
    }
}
