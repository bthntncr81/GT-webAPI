using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class imageq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectCount",
                table: "SubjectScheduleRelations",
                type: "integer",
                nullable: true);

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
                name: "IX_QuestionImage_SubjectScheduleRelationId",
                table: "QuestionImage",
                column: "SubjectScheduleRelationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionImage");

            migrationBuilder.DropColumn(
                name: "CorrectCount",
                table: "SubjectScheduleRelations");
        }
    }
}
