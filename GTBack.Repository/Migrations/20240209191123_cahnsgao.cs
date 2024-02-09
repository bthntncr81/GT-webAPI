using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class cahnsgao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mail",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Address");

            migrationBuilder.AlterColumn<int>(
                name: "TotalPrice",
                table: "ShoppingOrder",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "ShoppingOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderGuid",
                table: "ShoppingOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderNote",
                table: "ShoppingOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "ShoppingOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mail",
                table: "ShoppingOrder");

            migrationBuilder.DropColumn(
                name: "OrderGuid",
                table: "ShoppingOrder");

            migrationBuilder.DropColumn(
                name: "OrderNote",
                table: "ShoppingOrder");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "ShoppingOrder");

            migrationBuilder.AlterColumn<string>(
                name: "TotalPrice",
                table: "ShoppingOrder",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
