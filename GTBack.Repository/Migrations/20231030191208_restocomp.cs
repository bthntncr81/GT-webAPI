using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class restocomp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menu_RestoCompany_CompanyId",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "TableArea");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Table");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Department");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Menu",
                newName: "RestoCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Menu_CompanyId",
                table: "Menu",
                newName: "IX_Menu_RestoCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_RestoCompany_RestoCompanyId",
                table: "Menu",
                column: "RestoCompanyId",
                principalTable: "RestoCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menu_RestoCompany_RestoCompanyId",
                table: "Menu");

            migrationBuilder.RenameColumn(
                name: "RestoCompanyId",
                table: "Menu",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Menu_RestoCompanyId",
                table: "Menu",
                newName: "IX_Menu_CompanyId");

            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "TableArea",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "Table",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "Department",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_RestoCompany_CompanyId",
                table: "Menu",
                column: "CompanyId",
                principalTable: "RestoCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
