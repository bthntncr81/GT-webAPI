using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class global : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalProductModels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    ProductCode = table.Column<string>(type: "text", nullable: false),
                    Barcode = table.Column<string>(type: "text", nullable: false),
                    MainCategory = table.Column<string>(type: "text", nullable: false),
                    TopCategory = table.Column<string>(type: "text", nullable: false),
                    SubCategory = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    BrandId = table.Column<string>(type: "text", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Images = table.Column<string>(type: "text", nullable: false),
                    NotDiscountedPrice = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<string>(type: "text", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalProductModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Variants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Size = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<string>(type: "text", nullable: false),
                    VariantId = table.Column<string>(type: "text", nullable: false),
                    GlobalProductModelId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Variants_GlobalProductModels_GlobalProductModelId",
                        column: x => x.GlobalProductModelId,
                        principalTable: "GlobalProductModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Variants_GlobalProductModelId",
                table: "Variants",
                column: "GlobalProductModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Variants");

            migrationBuilder.DropTable(
                name: "GlobalProductModels");
        }
    }
}
