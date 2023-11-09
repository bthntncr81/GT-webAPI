using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class matris : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColumnCount",
                table: "TableArea",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RowCount",
                table: "TableArea",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColumnId",
                table: "Table",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RowId",
                table: "Table",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnCount",
                table: "TableArea");

            migrationBuilder.DropColumn(
                name: "RowCount",
                table: "TableArea");

            migrationBuilder.DropColumn(
                name: "ColumnId",
                table: "Table");

            migrationBuilder.DropColumn(
                name: "RowId",
                table: "Table");
        }
    }
}
