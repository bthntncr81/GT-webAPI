using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class varinat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Variants_GlobalProductModels_GlobalProductModelId",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Variants_GlobalProductModelId",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "GlobalProductModelId",
                table: "Variants");

            migrationBuilder.AddColumn<string>(
                name: "Variants",
                table: "GlobalProductModels",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Variants",
                table: "GlobalProductModels");

            migrationBuilder.AddColumn<long>(
                name: "GlobalProductModelId",
                table: "Variants",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Variants_GlobalProductModelId",
                table: "Variants",
                column: "GlobalProductModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Variants_GlobalProductModels_GlobalProductModelId",
                table: "Variants",
                column: "GlobalProductModelId",
                principalTable: "GlobalProductModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
