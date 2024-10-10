using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class pk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EcommerceVariantOrderRelation",
                table: "EcommerceVariantOrderRelation");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "EcommerceVariantOrderRelation",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EcommerceVariantOrderRelation",
                table: "EcommerceVariantOrderRelation",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceVariantOrderRelation_EcommerceVariantId",
                table: "EcommerceVariantOrderRelation",
                column: "EcommerceVariantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EcommerceVariantOrderRelation",
                table: "EcommerceVariantOrderRelation");

            migrationBuilder.DropIndex(
                name: "IX_EcommerceVariantOrderRelation_EcommerceVariantId",
                table: "EcommerceVariantOrderRelation");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "EcommerceVariantOrderRelation",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EcommerceVariantOrderRelation",
                table: "EcommerceVariantOrderRelation",
                columns: new[] { "EcommerceVariantId", "EcommerceOrderId" });
        }
    }
}
