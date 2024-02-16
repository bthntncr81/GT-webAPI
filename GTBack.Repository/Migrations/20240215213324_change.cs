using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ShoppingOrder",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "ShoppingOrder",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ShoppingOrder");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "ShoppingOrder");
        }
    }
}
