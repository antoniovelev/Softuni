using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentsSystem.Data.Migrations
{
    public partial class RefactoringModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Homeworks",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_UserId",
                table: "Homeworks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_AspNetUsers_UserId",
                table: "Homeworks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_AspNetUsers_UserId",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_UserId",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Homeworks");
        }
    }
}
