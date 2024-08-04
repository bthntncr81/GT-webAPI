using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class ecommerceinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EcommerceClientId",
                table: "RefreshToken",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EcommerceEmployeeId",
                table: "RefreshToken",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EcommerceBasket",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceBasket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EcommerceCompany",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Logo = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    GeoCodeY = table.Column<string>(type: "text", nullable: true),
                    GeoCodeX = table.Column<string>(type: "text", nullable: true),
                    ThemeId = table.Column<int>(type: "integer", nullable: false),
                    PrimaryColor = table.Column<string>(type: "text", nullable: true),
                    SecondaryColor = table.Column<string>(type: "text", nullable: true),
                    VergiNumber = table.Column<string>(type: "text", nullable: true),
                    IyzicoClientId = table.Column<string>(type: "text", nullable: true),
                    IyzicoSecretId = table.Column<string>(type: "text", nullable: true),
                    PrivacyPolicy = table.Column<string>(type: "text", nullable: true),
                    DeliveredAndReturnPolicy = table.Column<string>(type: "text", nullable: true),
                    DistanceSellingContract = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EcommerceClient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Logo = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    ActiveForgotLink = table.Column<string>(type: "text", nullable: true),
                    EcommerceCompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BasketId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceClient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EcommerceClient_EcommerceCompany_EcommerceCompanyId",
                        column: x => x.EcommerceCompanyId,
                        principalTable: "EcommerceCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcommerceEmployee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Logo = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    EcommerceCompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EcommerceEmployee_EcommerceCompany_EcommerceCompanyId",
                        column: x => x.EcommerceCompanyId,
                        principalTable: "EcommerceCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcommerceOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderGuid = table.Column<string>(type: "text", nullable: false),
                    EcommerceClientId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceClientId1 = table.Column<int>(type: "integer", nullable: false),
                    TotalPrice = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    OpenAddress = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    District = table.Column<string>(type: "text", nullable: false),
                    IyzicoTransactionId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EcommerceOrder_EcommerceClient_EcommerceClientId1",
                        column: x => x.EcommerceClientId1,
                        principalTable: "EcommerceClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcommerceProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category1 = table.Column<string>(type: "text", nullable: false),
                    Category2 = table.Column<string>(type: "text", nullable: false),
                    Category3 = table.Column<string>(type: "text", nullable: false),
                    EcommerceCompanyId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceEmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceEmployeeId1 = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EcommerceProduct_EcommerceCompany_EcommerceCompanyId",
                        column: x => x.EcommerceCompanyId,
                        principalTable: "EcommerceCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EcommerceProduct_EcommerceEmployee_EcommerceEmployeeId1",
                        column: x => x.EcommerceEmployeeId1,
                        principalTable: "EcommerceEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcommerceRefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    EcommerceEmployeeId = table.Column<int>(type: "integer", nullable: true),
                    EcommerceClientId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceRefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EcommerceRefreshToken_EcommerceClient_EcommerceClientId",
                        column: x => x.EcommerceClientId,
                        principalTable: "EcommerceClient",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EcommerceRefreshToken_EcommerceEmployee_EcommerceEmployeeId",
                        column: x => x.EcommerceEmployeeId,
                        principalTable: "EcommerceEmployee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EcommerceBasketProductRelation",
                columns: table => new
                {
                    EcommerceBasketId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceProductId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceBasketProductRelation", x => new { x.EcommerceBasketId, x.EcommerceProductId });
                    table.ForeignKey(
                        name: "FK_EcommerceBasketProductRelation_EcommerceBasket_EcommerceBas~",
                        column: x => x.EcommerceBasketId,
                        principalTable: "EcommerceBasket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EcommerceBasketProductRelation_EcommerceProduct_EcommercePr~",
                        column: x => x.EcommerceProductId,
                        principalTable: "EcommerceProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcommerceClientFavoriteRelation",
                columns: table => new
                {
                    EcommerceClientId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceProductId = table.Column<long>(type: "bigint", nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceClientFavoriteRelation", x => new { x.EcommerceClientId, x.EcommerceProductId });
                    table.ForeignKey(
                        name: "FK_EcommerceClientFavoriteRelation_EcommerceClient_ClientId",
                        column: x => x.ClientId,
                        principalTable: "EcommerceClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EcommerceClientFavoriteRelation_EcommerceProduct_EcommerceP~",
                        column: x => x.EcommerceProductId,
                        principalTable: "EcommerceProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcommerceProductOrderRelation",
                columns: table => new
                {
                    EcommerceProductId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceOrderId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceOrderId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceProductOrderRelation", x => new { x.EcommerceProductId, x.EcommerceOrderId });
                    table.ForeignKey(
                        name: "FK_EcommerceProductOrderRelation_EcommerceOrder_EcommerceOrder~",
                        column: x => x.EcommerceOrderId1,
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

            migrationBuilder.CreateTable(
                name: "EcommerceVariant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EcommerceProductId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Images = table.Column<string>(type: "text", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceVariant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EcommerceVariant_EcommerceProduct_EcommerceProductId",
                        column: x => x.EcommerceProductId,
                        principalTable: "EcommerceProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_EcommerceClientId",
                table: "RefreshToken",
                column: "EcommerceClientId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_EcommerceEmployeeId",
                table: "RefreshToken",
                column: "EcommerceEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceBasketProductRelation_EcommerceProductId",
                table: "EcommerceBasketProductRelation",
                column: "EcommerceProductId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceClient_EcommerceCompanyId",
                table: "EcommerceClient",
                column: "EcommerceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceClientFavoriteRelation_ClientId",
                table: "EcommerceClientFavoriteRelation",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceClientFavoriteRelation_EcommerceProductId",
                table: "EcommerceClientFavoriteRelation",
                column: "EcommerceProductId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceEmployee_EcommerceCompanyId",
                table: "EcommerceEmployee",
                column: "EcommerceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceOrder_EcommerceClientId1",
                table: "EcommerceOrder",
                column: "EcommerceClientId1");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceProduct_EcommerceCompanyId",
                table: "EcommerceProduct",
                column: "EcommerceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceProduct_EcommerceEmployeeId1",
                table: "EcommerceProduct",
                column: "EcommerceEmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceProductOrderRelation_EcommerceOrderId1",
                table: "EcommerceProductOrderRelation",
                column: "EcommerceOrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceRefreshToken_EcommerceClientId",
                table: "EcommerceRefreshToken",
                column: "EcommerceClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceRefreshToken_EcommerceEmployeeId",
                table: "EcommerceRefreshToken",
                column: "EcommerceEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceVariant_EcommerceProductId",
                table: "EcommerceVariant",
                column: "EcommerceProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_EcommerceClient_EcommerceClientId",
                table: "RefreshToken",
                column: "EcommerceClientId",
                principalTable: "EcommerceClient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_EcommerceEmployee_EcommerceEmployeeId",
                table: "RefreshToken",
                column: "EcommerceEmployeeId",
                principalTable: "EcommerceEmployee",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_EcommerceClient_EcommerceClientId",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_EcommerceEmployee_EcommerceEmployeeId",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "EcommerceBasketProductRelation");

            migrationBuilder.DropTable(
                name: "EcommerceClientFavoriteRelation");

            migrationBuilder.DropTable(
                name: "EcommerceProductOrderRelation");

            migrationBuilder.DropTable(
                name: "EcommerceRefreshToken");

            migrationBuilder.DropTable(
                name: "EcommerceVariant");

            migrationBuilder.DropTable(
                name: "EcommerceBasket");

            migrationBuilder.DropTable(
                name: "EcommerceOrder");

            migrationBuilder.DropTable(
                name: "EcommerceProduct");

            migrationBuilder.DropTable(
                name: "EcommerceClient");

            migrationBuilder.DropTable(
                name: "EcommerceEmployee");

            migrationBuilder.DropTable(
                name: "EcommerceCompany");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_EcommerceClientId",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_EcommerceEmployeeId",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "EcommerceClientId",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "EcommerceEmployeeId",
                table: "RefreshToken");
        }
    }
}
