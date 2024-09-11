using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class counquestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "SubjectScheduleRelations",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionCount",
                table: "SubjectScheduleRelations",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "SubjectScheduleRelations");

            migrationBuilder.DropColumn(
                name: "QuestionCount",
                table: "SubjectScheduleRelations");
        }
    }
}
