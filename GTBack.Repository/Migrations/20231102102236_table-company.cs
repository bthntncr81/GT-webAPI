using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class tablecompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Table_RestoCompany_RestoCompanyId",
                table: "Table");

            migrationBuilder.DropIndex(
                name: "IX_Table_RestoCompanyId",
                table: "Table");

            migrationBuilder.DropColumn(
                name: "RestoCompanyId",
                table: "Table");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RestoCompanyId",
                table: "Table",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Table_RestoCompanyId",
                table: "Table",
                column: "RestoCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Table_RestoCompany_RestoCompanyId",
                table: "Table",
                column: "RestoCompanyId",
                principalTable: "RestoCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
