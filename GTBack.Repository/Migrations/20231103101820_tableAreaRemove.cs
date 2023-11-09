using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class tableAreaRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addition_TableArea_TableAreaId",
                table: "Addition");

            migrationBuilder.DropIndex(
                name: "IX_Addition_TableAreaId",
                table: "Addition");

            migrationBuilder.DropColumn(
                name: "TableAreaId",
                table: "Addition");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TableAreaId",
                table: "Addition",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Addition_TableAreaId",
                table: "Addition",
                column: "TableAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addition_TableArea_TableAreaId",
                table: "Addition",
                column: "TableAreaId",
                principalTable: "TableArea",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
