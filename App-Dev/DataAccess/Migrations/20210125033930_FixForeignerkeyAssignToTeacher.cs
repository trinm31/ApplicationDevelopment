using Microsoft.EntityFrameworkCore.Migrations;

namespace App_Dev.DataAccess.Migrations
{
    public partial class FixForeignerkeyAssignToTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignToTrainers_Courses_CourseId",
                table: "CourseAssignToTrainers");

            migrationBuilder.DropColumn(
                name: "CourId",
                table: "CourseAssignToTrainers");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CourseAssignToTrainers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignToTrainers_Courses_CourseId",
                table: "CourseAssignToTrainers",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignToTrainers_Courses_CourseId",
                table: "CourseAssignToTrainers");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CourseAssignToTrainers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CourId",
                table: "CourseAssignToTrainers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignToTrainers_Courses_CourseId",
                table: "CourseAssignToTrainers",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
