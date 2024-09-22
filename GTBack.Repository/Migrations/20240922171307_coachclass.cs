using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class coachclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CoachId",
                table: "Classrooms",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_CoachId",
                table: "Classrooms",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_Coaches_CoachId",
                table: "Classrooms",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_Coaches_CoachId",
                table: "Classrooms");

            migrationBuilder.DropIndex(
                name: "IX_Classrooms_CoachId",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Classrooms");
        }
    }
}
