using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceProductOrderRelation_EcommerceOrder_EcommerceOrder~",
                table: "EcommerceProductOrderRelation");

            migrationBuilder.DropIndex(
                name: "IX_EcommerceProductOrderRelation_EcommerceOrderId1",
                table: "EcommerceProductOrderRelation");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "EcommerceVariant");

            migrationBuilder.DropColumn(
                name: "EcommerceOrderId1",
                table: "EcommerceProductOrderRelation");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "EcommerceVariant",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EcommerceVariant",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EcommerceVariant",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "EcommerceVariant",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "EcommerceOrder",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EcommerceOrder",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EcommerceOrder",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "EcommerceOrder",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EcommerceImage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<string>(type: "text", nullable: true),
                    EcommerceVariantId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EcommerceImage_EcommerceVariant_EcommerceVariantId",
                        column: x => x.EcommerceVariantId,
                        principalTable: "EcommerceVariant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceProductOrderRelation_EcommerceOrderId",
                table: "EcommerceProductOrderRelation",
                column: "EcommerceOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceImage_EcommerceVariantId",
                table: "EcommerceImage",
                column: "EcommerceVariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceProductOrderRelation_EcommerceOrder_EcommerceOrder~",
                table: "EcommerceProductOrderRelation",
                column: "EcommerceOrderId",
                principalTable: "EcommerceOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceProductOrderRelation_EcommerceOrder_EcommerceOrder~",
                table: "EcommerceProductOrderRelation");

            migrationBuilder.DropTable(
                name: "EcommerceImage");

            migrationBuilder.DropIndex(
                name: "IX_EcommerceProductOrderRelation_EcommerceOrderId",
                table: "EcommerceProductOrderRelation");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EcommerceVariant");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EcommerceVariant");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "EcommerceVariant");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EcommerceOrder");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EcommerceOrder");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "EcommerceOrder");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EcommerceVariant",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "EcommerceVariant",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EcommerceOrderId1",
                table: "EcommerceProductOrderRelation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EcommerceOrder",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceProductOrderRelation_EcommerceOrderId1",
                table: "EcommerceProductOrderRelation",
                column: "EcommerceOrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceProductOrderRelation_EcommerceOrder_EcommerceOrder~",
                table: "EcommerceProductOrderRelation",
                column: "EcommerceOrderId1",
                principalTable: "EcommerceOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
