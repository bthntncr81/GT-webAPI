using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTBack.Repository.Migrations
{
    public partial class EMPLOYEORDERPROCES : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addition_Employee_EmployeeId",
                table: "Addition");

            migrationBuilder.DropIndex(
                name: "IX_Addition_EmployeeId",
                table: "Addition");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Addition");

            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "OrderProcess",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProcess_EmployeeId",
                table: "OrderProcess",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProcess_Employee_EmployeeId",
                table: "OrderProcess",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProcess_Employee_EmployeeId",
                table: "OrderProcess");

            migrationBuilder.DropIndex(
                name: "IX_OrderProcess_EmployeeId",
                table: "OrderProcess");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "OrderProcess");

            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "Addition",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addition_EmployeeId",
                table: "Addition",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addition_Employee_EmployeeId",
                table: "Addition",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id");
        }
    }
}
