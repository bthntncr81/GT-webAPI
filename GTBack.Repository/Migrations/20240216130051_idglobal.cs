using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class idglobal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Variants_GlobalProductModels_GlobalProductModelId",
                table: "Variants");

            migrationBuilder.AlterColumn<long>(
                name: "GlobalProductModelId",
                table: "Variants",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Variants_GlobalProductModels_GlobalProductModelId",
                table: "Variants",
                column: "GlobalProductModelId",
                principalTable: "GlobalProductModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Variants_GlobalProductModels_GlobalProductModelId",
                table: "Variants");

            migrationBuilder.AlterColumn<long>(
                name: "GlobalProductModelId",
                table: "Variants",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Variants_GlobalProductModels_GlobalProductModelId",
                table: "Variants",
                column: "GlobalProductModelId",
                principalTable: "GlobalProductModels",
                principalColumn: "Id");
        }
    }
}
