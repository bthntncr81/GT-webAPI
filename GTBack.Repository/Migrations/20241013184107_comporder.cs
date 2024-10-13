using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class comporder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EcommerceCompanyId",
                table: "EcommerceOrder",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceOrder_EcommerceCompanyId",
                table: "EcommerceOrder",
                column: "EcommerceCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceOrder_EcommerceCompany_EcommerceCompanyId",
                table: "EcommerceOrder",
                column: "EcommerceCompanyId",
                principalTable: "EcommerceCompany",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceOrder_EcommerceCompany_EcommerceCompanyId",
                table: "EcommerceOrder");

            migrationBuilder.DropIndex(
                name: "IX_EcommerceOrder_EcommerceCompanyId",
                table: "EcommerceOrder");

            migrationBuilder.DropColumn(
                name: "EcommerceCompanyId",
                table: "EcommerceOrder");
        }
    }
}
