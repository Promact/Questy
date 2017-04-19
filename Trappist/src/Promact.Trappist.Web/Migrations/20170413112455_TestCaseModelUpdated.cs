using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Trappist.Web.Migrations
{
    public partial class TestCaseModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodeSnippetQuestionTestCases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CodeSnippetQuestionId = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    TestCaseDescription = table.Column<string>(nullable: true),
                    TestCaseInput = table.Column<string>(nullable: false),
                    TestCaseMarks = table.Column<double>(nullable: false),
                    TestCaseOutput = table.Column<string>(nullable: false),
                    TestCaseTitle = table.Column<string>(nullable: false),
                    TestCaseType = table.Column<int>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeSnippetQuestionTestCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeSnippetQuestionTestCases_CodeSnippetQuestion_CodeSnippetQuestionId",
                        column: x => x.CodeSnippetQuestionId,
                        principalTable: "CodeSnippetQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeSnippetQuestionTestCases_CodeSnippetQuestionId",
                table: "CodeSnippetQuestionTestCases",
                column: "CodeSnippetQuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeSnippetQuestionTestCases");
        }
    }
}
