using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class fcghjkl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VariantsName",
                table: "EcommerceProduct");

            migrationBuilder.AddColumn<string>(
                name: "VariantName",
                table: "EcommerceVariant",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VariantName",
                table: "EcommerceVariant");

            migrationBuilder.AddColumn<string>(
                name: "VariantsName",
                table: "EcommerceProduct",
                type: "text",
                nullable: true);
        }
    }
}
