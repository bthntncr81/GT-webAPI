using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class sruname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceProduct_EcommerceEmployee_EcommerceEmployeeId1",
                table: "EcommerceProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceRefreshToken_EcommerceEmployee_EcommerceEmployeeId",
                table: "EcommerceRefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_EcommerceRefreshToken_EcommerceEmployeeId",
                table: "EcommerceRefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_EcommerceProduct_EcommerceEmployeeId1",
                table: "EcommerceProduct");

            migrationBuilder.DropColumn(
                name: "EcommerceEmployeeId1",
                table: "EcommerceProduct");

            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "EcommerceEmployee",
                newName: "Surname");

            migrationBuilder.AlterColumn<long>(
                name: "EcommerceEmployeeId",
                table: "RefreshToken",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EcommerceEmployeeId1",
                table: "EcommerceRefreshToken",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "EcommerceEmployee",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "ActiveForgotLink",
                table: "EcommerceEmployee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "EcommerceEmployee",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceRefreshToken_EcommerceEmployeeId1",
                table: "EcommerceRefreshToken",
                column: "EcommerceEmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceProduct_EcommerceEmployeeId",
                table: "EcommerceProduct",
                column: "EcommerceEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceProduct_EcommerceEmployee_EcommerceEmployeeId",
                table: "EcommerceProduct",
                column: "EcommerceEmployeeId",
                principalTable: "EcommerceEmployee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceRefreshToken_EcommerceEmployee_EcommerceEmployeeId1",
                table: "EcommerceRefreshToken",
                column: "EcommerceEmployeeId1",
                principalTable: "EcommerceEmployee",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceProduct_EcommerceEmployee_EcommerceEmployeeId",
                table: "EcommerceProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceRefreshToken_EcommerceEmployee_EcommerceEmployeeId1",
                table: "EcommerceRefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_EcommerceRefreshToken_EcommerceEmployeeId1",
                table: "EcommerceRefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_EcommerceProduct_EcommerceEmployeeId",
                table: "EcommerceProduct");

            migrationBuilder.DropColumn(
                name: "EcommerceEmployeeId1",
                table: "EcommerceRefreshToken");

            migrationBuilder.DropColumn(
                name: "ActiveForgotLink",
                table: "EcommerceEmployee");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "EcommerceEmployee");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "EcommerceEmployee",
                newName: "Logo");

            migrationBuilder.AlterColumn<int>(
                name: "EcommerceEmployeeId",
                table: "RefreshToken",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EcommerceEmployeeId1",
                table: "EcommerceProduct",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EcommerceEmployee",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceRefreshToken_EcommerceEmployeeId",
                table: "EcommerceRefreshToken",
                column: "EcommerceEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceProduct_EcommerceEmployeeId1",
                table: "EcommerceProduct",
                column: "EcommerceEmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceProduct_EcommerceEmployee_EcommerceEmployeeId1",
                table: "EcommerceProduct",
                column: "EcommerceEmployeeId1",
                principalTable: "EcommerceEmployee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceRefreshToken_EcommerceEmployee_EcommerceEmployeeId",
                table: "EcommerceRefreshToken",
                column: "EcommerceEmployeeId",
                principalTable: "EcommerceEmployee",
                principalColumn: "Id");
        }
    }
}
