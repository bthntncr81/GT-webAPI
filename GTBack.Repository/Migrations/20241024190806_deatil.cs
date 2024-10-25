using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class deatil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountPageId",
                table: "EcommerceCompany",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeailPageId",
                table: "EcommerceCompany",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FooterId",
                table: "EcommerceCompany",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HeaderId",
                table: "EcommerceCompany",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoginRegisterPageId",
                table: "EcommerceCompany",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailPageId",
                table: "EcommerceCompany",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductCardId",
                table: "EcommerceCompany",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountPageId",
                table: "EcommerceCompany");

            migrationBuilder.DropColumn(
                name: "DeailPageId",
                table: "EcommerceCompany");

            migrationBuilder.DropColumn(
                name: "FooterId",
                table: "EcommerceCompany");

            migrationBuilder.DropColumn(
                name: "HeaderId",
                table: "EcommerceCompany");

            migrationBuilder.DropColumn(
                name: "LoginRegisterPageId",
                table: "EcommerceCompany");

            migrationBuilder.DropColumn(
                name: "OrderDetailPageId",
                table: "EcommerceCompany");

            migrationBuilder.DropColumn(
                name: "ProductCardId",
                table: "EcommerceCompany");
        }
    }
}
