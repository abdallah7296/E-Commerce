using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_come.Migrations
{
    /// <inheritdoc />
    public partial class cartsss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_AspNetUsers_UserId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.RenameTable(
                name: "OrderDetails",
                newName: "userCarts");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_UserId",
                table: "userCarts",
                newName: "IX_userCarts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductId",
                table: "userCarts",
                newName: "IX_userCarts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userCarts",
                table: "userCarts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_userCarts_AspNetUsers_UserId",
                table: "userCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_userCarts_products_ProductId",
                table: "userCarts",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userCarts_AspNetUsers_UserId",
                table: "userCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_userCarts_products_ProductId",
                table: "userCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userCarts",
                table: "userCarts");

            migrationBuilder.RenameTable(
                name: "userCarts",
                newName: "OrderDetails");

            migrationBuilder.RenameIndex(
                name: "IX_userCarts_UserId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_userCarts_ProductId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_AspNetUsers_UserId",
                table: "OrderDetails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
