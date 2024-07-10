using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class sho_order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShoppingOrder_AddressId",
                table: "ShoppingOrder");

            migrationBuilder.DropColumn(
                name: "ShoppingOrderId",
                table: "Address");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingOrder_AddressId",
                table: "ShoppingOrder",
                column: "AddressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShoppingOrder_AddressId",
                table: "ShoppingOrder");

            migrationBuilder.AddColumn<long>(
                name: "ShoppingOrderId",
                table: "Address",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingOrder_AddressId",
                table: "ShoppingOrder",
                column: "AddressId",
                unique: true);
        }
    }
}
