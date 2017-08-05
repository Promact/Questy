using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Trappist.Web.Migrations
{
    public partial class Added_TestCodeSolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestCodeSolution",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Language = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    Score = table.Column<double>(nullable: false),
                    Solution = table.Column<string>(nullable: true),
                    TestAttendeeId = table.Column<int>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCodeSolution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCodeSolution_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestCodeSolution_TestAttendees_TestAttendeeId",
                        column: x => x.TestAttendeeId,
                        principalTable: "TestAttendees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestCodeSolution_QuestionId",
                table: "TestCodeSolution",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCodeSolution_TestAttendeeId",
                table: "TestCodeSolution",
                column: "TestAttendeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestCodeSolution");
        }
    }
}
