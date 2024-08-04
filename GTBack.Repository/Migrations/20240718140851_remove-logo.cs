using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class removelogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceClientFavoriteRelation_EcommerceClient_ClientId",
                table: "EcommerceClientFavoriteRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceOrder_EcommerceClient_EcommerceClientId1",
                table: "EcommerceOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceRefreshToken_EcommerceClient_EcommerceClientId",
                table: "EcommerceRefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_EcommerceRefreshToken_EcommerceClientId",
                table: "EcommerceRefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_EcommerceOrder_EcommerceClientId1",
                table: "EcommerceOrder");

            migrationBuilder.DropIndex(
                name: "IX_EcommerceClientFavoriteRelation_ClientId",
                table: "EcommerceClientFavoriteRelation");

            migrationBuilder.DropColumn(
                name: "EcommerceClientId1",
                table: "EcommerceOrder");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "EcommerceClientFavoriteRelation");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "EcommerceClient");

            migrationBuilder.AlterColumn<long>(
                name: "EcommerceClientId",
                table: "RefreshToken",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EcommerceClientId1",
                table: "EcommerceRefreshToken",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "EcommerceClient",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceRefreshToken_EcommerceClientId1",
                table: "EcommerceRefreshToken",
                column: "EcommerceClientId1");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceOrder_EcommerceClientId",
                table: "EcommerceOrder",
                column: "EcommerceClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceClientFavoriteRelation_EcommerceClient_EcommerceCl~",
                table: "EcommerceClientFavoriteRelation",
                column: "EcommerceClientId",
                principalTable: "EcommerceClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceOrder_EcommerceClient_EcommerceClientId",
                table: "EcommerceOrder",
                column: "EcommerceClientId",
                principalTable: "EcommerceClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceRefreshToken_EcommerceClient_EcommerceClientId1",
                table: "EcommerceRefreshToken",
                column: "EcommerceClientId1",
                principalTable: "EcommerceClient",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceClientFavoriteRelation_EcommerceClient_EcommerceCl~",
                table: "EcommerceClientFavoriteRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceOrder_EcommerceClient_EcommerceClientId",
                table: "EcommerceOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_EcommerceRefreshToken_EcommerceClient_EcommerceClientId1",
                table: "EcommerceRefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_EcommerceRefreshToken_EcommerceClientId1",
                table: "EcommerceRefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_EcommerceOrder_EcommerceClientId",
                table: "EcommerceOrder");

            migrationBuilder.DropColumn(
                name: "EcommerceClientId1",
                table: "EcommerceRefreshToken");

            migrationBuilder.AlterColumn<int>(
                name: "EcommerceClientId",
                table: "RefreshToken",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EcommerceClientId1",
                table: "EcommerceOrder",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "EcommerceClientFavoriteRelation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EcommerceClient",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "EcommerceClient",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceRefreshToken_EcommerceClientId",
                table: "EcommerceRefreshToken",
                column: "EcommerceClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceOrder_EcommerceClientId1",
                table: "EcommerceOrder",
                column: "EcommerceClientId1");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceClientFavoriteRelation_ClientId",
                table: "EcommerceClientFavoriteRelation",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceClientFavoriteRelation_EcommerceClient_ClientId",
                table: "EcommerceClientFavoriteRelation",
                column: "ClientId",
                principalTable: "EcommerceClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceOrder_EcommerceClient_EcommerceClientId1",
                table: "EcommerceOrder",
                column: "EcommerceClientId1",
                principalTable: "EcommerceClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EcommerceRefreshToken_EcommerceClient_EcommerceClientId",
                table: "EcommerceRefreshToken",
                column: "EcommerceClientId",
                principalTable: "EcommerceClient",
                principalColumn: "Id");
        }
    }
}
