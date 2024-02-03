using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class checkout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingOrder_Basket_BasketId",
                table: "ShoppingOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingOrder_Product_ProductId",
                table: "ShoppingOrder");

            migrationBuilder.DropTable(
                name: "Basket");

            migrationBuilder.DropTable(
                name: "Favorite");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingOrder_BasketId",
                table: "ShoppingOrder");

            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "ShoppingOrder");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ShoppingOrder",
                newName: "ShoppingUserId");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "ShoppingOrder",
                newName: "TotalPrice");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingOrder_ProductId",
                table: "ShoppingOrder",
                newName: "IX_ShoppingOrder_ShoppingUserId");

            migrationBuilder.AddColumn<string>(
                name: "BasketJsonDetail",
                table: "ShoppingOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "ShoppingOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IyzicoTransactionId",
                table: "ShoppingOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderDate",
                table: "ShoppingOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ShoppingOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingOrder_ShoppingUser_ShoppingUserId",
                table: "ShoppingOrder",
                column: "ShoppingUserId",
                principalTable: "ShoppingUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingOrder_ShoppingUser_ShoppingUserId",
                table: "ShoppingOrder");

            migrationBuilder.DropColumn(
                name: "BasketJsonDetail",
                table: "ShoppingOrder");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "ShoppingOrder");

            migrationBuilder.DropColumn(
                name: "IyzicoTransactionId",
                table: "ShoppingOrder");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "ShoppingOrder");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ShoppingOrder");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "ShoppingOrder",
                newName: "Data");

            migrationBuilder.RenameColumn(
                name: "ShoppingUserId",
                table: "ShoppingOrder",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingOrder_ShoppingUserId",
                table: "ShoppingOrder",
                newName: "IX_ShoppingOrder_ProductId");

            migrationBuilder.AddColumn<long>(
                name: "BasketId",
                table: "ShoppingOrder",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Basket",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingUserId1 = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ShoppingUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Basket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Basket_ShoppingUser_ShoppingUserId1",
                        column: x => x.ShoppingUserId1,
                        principalTable: "ShoppingUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorite",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId1 = table.Column<long>(type: "bigint", nullable: false),
                    ShoppingUserId1 = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ShoppingUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorite_Product_ProductId1",
                        column: x => x.ProductId1,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorite_ShoppingUser_ShoppingUserId1",
                        column: x => x.ShoppingUserId1,
                        principalTable: "ShoppingUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingOrder_BasketId",
                table: "ShoppingOrder",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_Basket_ShoppingUserId1",
                table: "Basket",
                column: "ShoppingUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_ProductId1",
                table: "Favorite",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_ShoppingUserId1",
                table: "Favorite",
                column: "ShoppingUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingOrder_Basket_BasketId",
                table: "ShoppingOrder",
                column: "BasketId",
                principalTable: "Basket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingOrder_Product_ProductId",
                table: "ShoppingOrder",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
