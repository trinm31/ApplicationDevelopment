using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App_Dev.DataAccess.Migrations
{
    public partial class fiName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseAssignToTrainers");

            migrationBuilder.CreateTable(
                name: "courseTrainers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(nullable: false),
                    TrainerId = table.Column<string>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courseTrainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_courseTrainers_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_courseTrainers_AspNetUsers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_courseTrainers_CourseId",
                table: "courseTrainers",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_courseTrainers_TrainerId",
                table: "courseTrainers",
                column: "TrainerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "courseTrainers");

            migrationBuilder.CreateTable(
                name: "CourseAssignToTrainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrainerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAssignToTrainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseAssignToTrainers_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseAssignToTrainers_AspNetUsers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssignToTrainers_CourseId",
                table: "CourseAssignToTrainers",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssignToTrainers_TrainerId",
                table: "CourseAssignToTrainers",
                column: "TrainerId");
        }
    }
}
