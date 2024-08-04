using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class indbrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VariantIndicator",
                table: "EcommerceVariant",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "EcommerceProduct",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VariantIndicator",
                table: "EcommerceVariant");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "EcommerceProduct");
        }
    }
}
