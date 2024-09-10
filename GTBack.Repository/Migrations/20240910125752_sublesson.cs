using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class sublesson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SubLessonId",
                table: "Schedules",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SubLessonId",
                table: "Schedules",
                column: "SubLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_SubLessons_SubLessonId",
                table: "Schedules",
                column: "SubLessonId",
                principalTable: "SubLessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_SubLessons_SubLessonId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_SubLessonId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "SubLessonId",
                table: "Schedules");
        }
    }
}
