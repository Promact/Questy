using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Trappist.Web.Migrations
{
    public partial class AddedModelForConductingTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "AttendeeAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Answers = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendeeAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendeeAnswers_TestAttendees_Id",
                        column: x => x.Id,
                        principalTable: "TestAttendees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestConduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    QuestionStatus = table.Column<int>(nullable: false),
                    TestAttendeeId = table.Column<int>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestConduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestConduct_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestConduct_TestAttendees_TestAttendeeId",
                        column: x => x.TestAttendeeId,
                        principalTable: "TestAttendees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnsweredCodeSnippet = table.Column<string>(nullable: true),
                    AnsweredOption = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    TestConductId = table.Column<int>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestAnswers_TestConduct_TestConductId",
                        column: x => x.TestConductId,
                        principalTable: "TestConduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestAnswers_TestConductId",
                table: "TestAnswers",
                column: "TestConductId");

            migrationBuilder.CreateIndex(
                name: "IX_TestConduct_QuestionId",
                table: "TestConduct",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestConduct_TestAttendeeId",
                table: "TestConduct",
                column: "TestAttendeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendeeAnswers");

            migrationBuilder.DropTable(
                name: "TestAnswers");

            migrationBuilder.DropTable(
                name: "TestConduct");
        }
    }
}
