using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class OrderRel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EcommerceProductOrderRelation");

            migrationBuilder.CreateTable(
                name: "EcommerceVariantOrderRelation",
                columns: table => new
                {
                    EcommerceVariantId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceOrderId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceProductId = table.Column<long>(type: "bigint", nullable: true),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceVariantOrderRelation", x => new { x.EcommerceVariantId, x.EcommerceOrderId });
                    table.ForeignKey(
                        name: "FK_EcommerceVariantOrderRelation_EcommerceOrder_EcommerceOrder~",
                        column: x => x.EcommerceOrderId,
                        principalTable: "EcommerceOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EcommerceVariantOrderRelation_EcommerceProduct_EcommercePro~",
                        column: x => x.EcommerceProductId,
                        principalTable: "EcommerceProduct",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EcommerceVariantOrderRelation_EcommerceVariant_EcommerceVar~",
                        column: x => x.EcommerceVariantId,
                        principalTable: "EcommerceVariant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceVariantOrderRelation_EcommerceOrderId",
                table: "EcommerceVariantOrderRelation",
                column: "EcommerceOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceVariantOrderRelation_EcommerceProductId",
                table: "EcommerceVariantOrderRelation",
                column: "EcommerceProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EcommerceVariantOrderRelation");

            migrationBuilder.CreateTable(
                name: "EcommerceProductOrderRelation",
                columns: table => new
                {
                    EcommerceProductId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceOrderId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceProductOrderRelation", x => new { x.EcommerceProductId, x.EcommerceOrderId });
                    table.ForeignKey(
                        name: "FK_EcommerceProductOrderRelation_EcommerceOrder_EcommerceOrder~",
                        column: x => x.EcommerceOrderId,
                        principalTable: "EcommerceOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EcommerceProductOrderRelation_EcommerceProduct_EcommercePro~",
                        column: x => x.EcommerceProductId,
                        principalTable: "EcommerceProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceProductOrderRelation_EcommerceOrderId",
                table: "EcommerceProductOrderRelation",
                column: "EcommerceOrderId");
        }
    }
}
