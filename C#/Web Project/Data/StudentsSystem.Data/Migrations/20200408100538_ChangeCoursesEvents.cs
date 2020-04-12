using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentsSystem.Data.Migrations
{
    public partial class ChangeCoursesEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "CoursesEvents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "CoursesEvents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "CoursesEvents",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CoursesEvents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "CoursesEvents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoursesEvents_IsDeleted",
                table: "CoursesEvents",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CoursesEvents_IsDeleted",
                table: "CoursesEvents");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "CoursesEvents");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "CoursesEvents");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CoursesEvents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CoursesEvents");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "CoursesEvents");
        }
    }
}
