using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class companyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingUser_Product_ProductId",
                table: "ShoppingUser");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingUser_ProductId",
                table: "ShoppingUser");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ShoppingUser");

            migrationBuilder.AddColumn<long>(
                name: "ShoppingCompanyId",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ShoppingCompanyId",
                table: "Product",
                column: "ShoppingCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ShoppingCompany_ShoppingCompanyId",
                table: "Product",
                column: "ShoppingCompanyId",
                principalTable: "ShoppingCompany",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ShoppingCompany_ShoppingCompanyId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ShoppingCompanyId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ShoppingCompanyId",
                table: "Product");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ShoppingUser",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingUser_ProductId",
                table: "ShoppingUser",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingUser_Product_ProductId",
                table: "ShoppingUser",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }
    }
}
