using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class variant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceBasketProductRelation_EcommerceProduct_EcommercePr~",
                table: "EcommerceBasketProductRelation");

            migrationBuilder.RenameColumn(
                name: "EcommerceProductId",
                table: "EcommerceBasketProductRelation",
                newName: "EcommerceVariantId");

            migrationBuilder.RenameIndex(
                name: "IX_EcommerceBasketProductRelation_EcommerceProductId",
                table: "EcommerceBasketProductRelation",
                newName: "IX_EcommerceBasketProductRelation_EcommerceVariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceBasketProductRelation_EcommerceVariant_EcommerceVa~",
                table: "EcommerceBasketProductRelation",
                column: "EcommerceVariantId",
                principalTable: "EcommerceVariant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceBasketProductRelation_EcommerceVariant_EcommerceVa~",
                table: "EcommerceBasketProductRelation");

            migrationBuilder.RenameColumn(
                name: "EcommerceVariantId",
                table: "EcommerceBasketProductRelation",
                newName: "EcommerceProductId");

            migrationBuilder.RenameIndex(
                name: "IX_EcommerceBasketProductRelation_EcommerceVariantId",
                table: "EcommerceBasketProductRelation",
                newName: "IX_EcommerceBasketProductRelation_EcommerceProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceBasketProductRelation_EcommerceProduct_EcommercePr~",
                table: "EcommerceBasketProductRelation",
                column: "EcommerceProductId",
                principalTable: "EcommerceProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
