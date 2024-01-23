using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class changesonuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingUser_ShoppingCompany_ShoppingCompanyId",
                table: "ShoppingUser");

            migrationBuilder.AlterColumn<long>(
                name: "ShoppingCompanyId",
                table: "ShoppingUser",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                table: "ShoppingUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingUser_ShoppingCompany_ShoppingCompanyId",
                table: "ShoppingUser",
                column: "ShoppingCompanyId",
                principalTable: "ShoppingCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingUser_ShoppingCompany_ShoppingCompanyId",
                table: "ShoppingUser");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                table: "ShoppingUser");

            migrationBuilder.AlterColumn<long>(
                name: "ShoppingCompanyId",
                table: "ShoppingUser",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingUser_ShoppingCompany_ShoppingCompanyId",
                table: "ShoppingUser",
                column: "ShoppingCompanyId",
                principalTable: "ShoppingCompany",
                principalColumn: "Id");
        }
    }
}
