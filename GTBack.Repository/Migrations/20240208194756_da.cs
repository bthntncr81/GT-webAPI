using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class da : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingOrder_ShoppingUser_ShoppingUserId",
                table: "ShoppingOrder");

            migrationBuilder.DropTable(
                name: "XmlFiles");

            migrationBuilder.AlterColumn<long>(
                name: "ShoppingUserId",
                table: "ShoppingOrder",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "AddressId",
                table: "ShoppingOrder",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpenAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShoppingOrderId = table.Column<long>(type: "bigint", nullable: true),
                    ShoppingUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_ShoppingUser_ShoppingUserId",
                        column: x => x.ShoppingUserId,
                        principalTable: "ShoppingUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingOrder_AddressId",
                table: "ShoppingOrder",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Address_ShoppingUserId",
                table: "Address",
                column: "ShoppingUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingOrder_Address_AddressId",
                table: "ShoppingOrder",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingOrder_ShoppingUser_ShoppingUserId",
                table: "ShoppingOrder",
                column: "ShoppingUserId",
                principalTable: "ShoppingUser",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingOrder_Address_AddressId",
                table: "ShoppingOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingOrder_ShoppingUser_ShoppingUserId",
                table: "ShoppingOrder");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingOrder_AddressId",
                table: "ShoppingOrder");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "ShoppingOrder");

            migrationBuilder.AlterColumn<long>(
                name: "ShoppingUserId",
                table: "ShoppingOrder",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "XmlFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    file = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XmlFiles", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingOrder_ShoppingUser_ShoppingUserId",
                table: "ShoppingOrder",
                column: "ShoppingUserId",
                principalTable: "ShoppingUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
