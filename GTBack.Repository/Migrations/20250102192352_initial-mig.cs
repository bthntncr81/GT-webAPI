using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class initialmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    ActiveCoachGuid = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    ActiveForgotLink = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Desc = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EcommerceBasket",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<string>(type: "text", nullable: false),
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
                    WebAddress = table.Column<string>(type: "text", nullable: true),
                    EmailPassword = table.Column<string>(type: "text", nullable: true),
                    SmtpServer = table.Column<string>(type: "text", nullable: true),
                    SmtpPort = table.Column<int>(type: "integer", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    GeoCodeY = table.Column<string>(type: "text", nullable: true),
                    GeoCodeX = table.Column<string>(type: "text", nullable: true),
                    ThemeId = table.Column<int>(type: "integer", nullable: false),
                    ProductCardId = table.Column<int>(type: "integer", nullable: true),
                    DeailPageId = table.Column<int>(type: "integer", nullable: true),
                    HeaderId = table.Column<int>(type: "integer", nullable: true),
                    FooterId = table.Column<int>(type: "integer", nullable: true),
                    AccountPageId = table.Column<int>(type: "integer", nullable: true),
                    OrderDetailPageId = table.Column<int>(type: "integer", nullable: true),
                    LoginRegisterPageId = table.Column<int>(type: "integer", nullable: true),
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
                name: "GlobalProductModels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<string>(type: "text", nullable: true),
                    ProductCode = table.Column<string>(type: "text", nullable: true),
                    Barcode = table.Column<string>(type: "text", nullable: false),
                    MainCategory = table.Column<string>(type: "text", nullable: true),
                    TopCategory = table.Column<string>(type: "text", nullable: true),
                    SubCategory = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true),
                    BrandId = table.Column<string>(type: "text", nullable: true),
                    Brand = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<string>(type: "text", nullable: true),
                    NotDiscountedPrice = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<string>(type: "text", nullable: true),
                    Variants = table.Column<string>(type: "text", nullable: true),
                    Detail = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalProductModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCompany",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Mail = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Logo = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CoachId = table.Column<long>(type: "bigint", nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classrooms_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcommerceClient",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    ActiveForgotLink = table.Column<string>(type: "text", nullable: true),
                    EcommerceCompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BasketId = table.Column<long>(type: "bigint", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    ActiveForgotLink = table.Column<string>(type: "text", nullable: true),
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
                name: "SubLessons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LessonId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubLessons_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    MainCategory = table.Column<string>(type: "text", nullable: false),
                    SubCategory = table.Column<string>(type: "text", nullable: false),
                    TopCategory = table.Column<string>(type: "text", nullable: false),
                    ShoppingCompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_ShoppingCompany_ShoppingCompanyId",
                        column: x => x.ShoppingCompanyId,
                        principalTable: "ShoppingCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Mail = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Favorites = table.Column<string>(type: "text", nullable: false),
                    ActiveBasketId = table.Column<long>(type: "bigint", nullable: true),
                    ShoppingCompanyId = table.Column<long>(type: "bigint", nullable: false),
                    ActiveForgotLink = table.Column<string>(type: "text", nullable: true),
                    UserTypeId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingUser_ShoppingCompany_ShoppingCompanyId",
                        column: x => x.ShoppingCompanyId,
                        principalTable: "ShoppingCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: false),
                    CoachId = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    ClassroomId = table.Column<long>(type: "bigint", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    ActiveForgotLink = table.Column<string>(type: "text", nullable: true),
                    HavePermission = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcommerceOrder",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderGuid = table.Column<string>(type: "text", nullable: false),
                    EcommerceClientId = table.Column<long>(type: "bigint", nullable: true),
                    EcommerceCompanyId = table.Column<long>(type: "bigint", nullable: true),
                    TotalPrice = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false),
                    ShippmentTrackingLink = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    OpenAddress = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    District = table.Column<string>(type: "text", nullable: false),
                    IyzicoTransactionId = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EcommerceOrder_EcommerceClient_EcommerceClientId",
                        column: x => x.EcommerceClientId,
                        principalTable: "EcommerceClient",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EcommerceOrder_EcommerceCompany_EcommerceCompanyId",
                        column: x => x.EcommerceCompanyId,
                        principalTable: "EcommerceCompany",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EcommerceProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category1 = table.Column<string>(type: "text", nullable: false),
                    Category2 = table.Column<string>(type: "text", nullable: true),
                    Category3 = table.Column<string>(type: "text", nullable: true),
                    Brand = table.Column<string>(type: "text", nullable: true),
                    EcommerceCompanyId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceEmployeeId = table.Column<long>(type: "bigint", nullable: false),
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
                        name: "FK_EcommerceProduct_EcommerceEmployee_EcommerceEmployeeId",
                        column: x => x.EcommerceEmployeeId,
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
                    EcommerceEmployeeId1 = table.Column<long>(type: "bigint", nullable: true),
                    EcommerceEmployeeId = table.Column<int>(type: "integer", nullable: true),
                    EcommerceClientId1 = table.Column<long>(type: "bigint", nullable: true),
                    EcommerceClientId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceRefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EcommerceRefreshToken_EcommerceClient_EcommerceClientId1",
                        column: x => x.EcommerceClientId1,
                        principalTable: "EcommerceClient",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EcommerceRefreshToken_EcommerceEmployee_EcommerceEmployeeId1",
                        column: x => x.EcommerceEmployeeId1,
                        principalTable: "EcommerceEmployee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubLessonId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_SubLessons_SubLessonId",
                        column: x => x.SubLessonId,
                        principalTable: "SubLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    District = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    OpenAddress = table.Column<string>(type: "text", nullable: false),
                    ShoppingUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    InitialPassword = table.Column<string>(type: "text", nullable: false),
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    ActiveForgotLink = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    customerId = table.Column<int>(type: "integer", nullable: true),
                    CoachId = table.Column<long>(type: "bigint", nullable: true),
                    EcommerceClientId = table.Column<long>(type: "bigint", nullable: true),
                    EcommerceEmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    StudentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RefreshToken_EcommerceClient_EcommerceClientId",
                        column: x => x.EcommerceClientId,
                        principalTable: "EcommerceClient",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RefreshToken_EcommerceEmployee_EcommerceEmployeeId",
                        column: x => x.EcommerceEmployeeId,
                        principalTable: "EcommerceEmployee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RefreshToken_ShoppingUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ShoppingUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RefreshToken_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    SubLessonId = table.Column<long>(type: "bigint", nullable: false),
                    UniqueId = table.Column<string>(type: "text", nullable: true),
                    TimeSlot = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_SubLessons_SubLessonId",
                        column: x => x.SubLessonId,
                        principalTable: "SubLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcommerceClientFavoriteRelation",
                columns: table => new
                {
                    EcommerceClientId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceProductId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceClientFavoriteRelation", x => new { x.EcommerceClientId, x.EcommerceProductId });
                    table.ForeignKey(
                        name: "FK_EcommerceClientFavoriteRelation_EcommerceClient_EcommerceCl~",
                        column: x => x.EcommerceClientId,
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
                name: "EcommerceVariant",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EcommerceProductId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ThumbImage = table.Column<string>(type: "text", nullable: true),
                    VariantCode = table.Column<string>(type: "text", nullable: true),
                    VariantName = table.Column<string>(type: "text", nullable: true),
                    VariantIndicator = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ShoppingOrder",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BasketJsonDetail = table.Column<string>(type: "text", nullable: false),
                    OrderGuid = table.Column<string>(type: "text", nullable: false),
                    ShoppingUserId = table.Column<long>(type: "bigint", nullable: true),
                    TotalPrice = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    IyzicoTransactionId = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    OrderNote = table.Column<string>(type: "text", nullable: false),
                    Mail = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AddressId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingOrder_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingOrder_ShoppingUser_ShoppingUserId",
                        column: x => x.ShoppingUserId,
                        principalTable: "ShoppingUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubjectScheduleRelations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScheduleId = table.Column<long>(type: "bigint", nullable: true),
                    SubjectId = table.Column<long>(type: "bigint", nullable: true),
                    QuestionCount = table.Column<int>(type: "integer", nullable: true),
                    UniqueId = table.Column<string>(type: "text", nullable: true),
                    CorrectCount = table.Column<int>(type: "integer", nullable: true),
                    IsDone = table.Column<bool>(type: "boolean", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectScheduleRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectScheduleRelations_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectScheduleRelations_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcommerceBasketProductRelation",
                columns: table => new
                {
                    EcommerceBasketId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceVariantId = table.Column<long>(type: "bigint", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceBasketProductRelation", x => new { x.EcommerceBasketId, x.EcommerceVariantId });
                    table.ForeignKey(
                        name: "FK_EcommerceBasketProductRelation_EcommerceBasket_EcommerceBas~",
                        column: x => x.EcommerceBasketId,
                        principalTable: "EcommerceBasket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EcommerceBasketProductRelation_EcommerceVariant_EcommerceVa~",
                        column: x => x.EcommerceVariantId,
                        principalTable: "EcommerceVariant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "EcommerceVariantOrderRelation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EcommerceVariantId = table.Column<long>(type: "bigint", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    EcommerceOrderId = table.Column<long>(type: "bigint", nullable: false),
                    EcommerceProductId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcommerceVariantOrderRelation", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "QuestionImage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<string>(type: "text", nullable: false),
                    SubjectScheduleRelationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionImage_SubjectScheduleRelations_SubjectScheduleRelat~",
                        column: x => x.SubjectScheduleRelationId,
                        principalTable: "SubjectScheduleRelations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_ShoppingUserId",
                table: "Address",
                column: "ShoppingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_CoachId",
                table: "Classrooms",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceBasketProductRelation_EcommerceVariantId",
                table: "EcommerceBasketProductRelation",
                column: "EcommerceVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceClient_EcommerceCompanyId",
                table: "EcommerceClient",
                column: "EcommerceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceClientFavoriteRelation_EcommerceProductId",
                table: "EcommerceClientFavoriteRelation",
                column: "EcommerceProductId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceEmployee_EcommerceCompanyId",
                table: "EcommerceEmployee",
                column: "EcommerceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceImage_EcommerceVariantId",
                table: "EcommerceImage",
                column: "EcommerceVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceOrder_EcommerceClientId",
                table: "EcommerceOrder",
                column: "EcommerceClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceOrder_EcommerceCompanyId",
                table: "EcommerceOrder",
                column: "EcommerceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceProduct_EcommerceCompanyId",
                table: "EcommerceProduct",
                column: "EcommerceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceProduct_EcommerceEmployeeId",
                table: "EcommerceProduct",
                column: "EcommerceEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceRefreshToken_EcommerceClientId1",
                table: "EcommerceRefreshToken",
                column: "EcommerceClientId1");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceRefreshToken_EcommerceEmployeeId1",
                table: "EcommerceRefreshToken",
                column: "EcommerceEmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceVariant_EcommerceProductId",
                table: "EcommerceVariant",
                column: "EcommerceProductId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceVariantOrderRelation_EcommerceOrderId",
                table: "EcommerceVariantOrderRelation",
                column: "EcommerceOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceVariantOrderRelation_EcommerceProductId",
                table: "EcommerceVariantOrderRelation",
                column: "EcommerceProductId");

            migrationBuilder.CreateIndex(
                name: "IX_EcommerceVariantOrderRelation_EcommerceVariantId",
                table: "EcommerceVariantOrderRelation",
                column: "EcommerceVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_ProductId",
                table: "Image",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_StudentId",
                table: "Parents",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ShoppingCompanyId",
                table: "Product",
                column: "ShoppingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionImage_SubjectScheduleRelationId",
                table: "QuestionImage",
                column: "SubjectScheduleRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_CoachId",
                table: "RefreshToken",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_EcommerceClientId",
                table: "RefreshToken",
                column: "EcommerceClientId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_EcommerceEmployeeId",
                table: "RefreshToken",
                column: "EcommerceEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_StudentId",
                table: "RefreshToken",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_StudentId",
                table: "Schedules",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SubLessonId",
                table: "Schedules",
                column: "SubLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingOrder_AddressId",
                table: "ShoppingOrder",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingOrder_ShoppingUserId",
                table: "ShoppingOrder",
                column: "ShoppingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingUser_ShoppingCompanyId",
                table: "ShoppingUser",
                column: "ShoppingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassroomId",
                table: "Students",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CoachId",
                table: "Students",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SubLessonId",
                table: "Subjects",
                column: "SubLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectScheduleRelations_ScheduleId",
                table: "SubjectScheduleRelations",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectScheduleRelations_SubjectId",
                table: "SubjectScheduleRelations",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubLessons_LessonId",
                table: "SubLessons",
                column: "LessonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "EcommerceBasketProductRelation");

            migrationBuilder.DropTable(
                name: "EcommerceClientFavoriteRelation");

            migrationBuilder.DropTable(
                name: "EcommerceImage");

            migrationBuilder.DropTable(
                name: "EcommerceRefreshToken");

            migrationBuilder.DropTable(
                name: "EcommerceVariantOrderRelation");

            migrationBuilder.DropTable(
                name: "GlobalProductModels");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropTable(
                name: "QuestionImage");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "ShoppingOrder");

            migrationBuilder.DropTable(
                name: "EcommerceBasket");

            migrationBuilder.DropTable(
                name: "EcommerceOrder");

            migrationBuilder.DropTable(
                name: "EcommerceVariant");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "SubjectScheduleRelations");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "EcommerceClient");

            migrationBuilder.DropTable(
                name: "EcommerceProduct");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "ShoppingUser");

            migrationBuilder.DropTable(
                name: "EcommerceEmployee");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "SubLessons");

            migrationBuilder.DropTable(
                name: "ShoppingCompany");

            migrationBuilder.DropTable(
                name: "EcommerceCompany");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Coaches");
        }
    }
}
