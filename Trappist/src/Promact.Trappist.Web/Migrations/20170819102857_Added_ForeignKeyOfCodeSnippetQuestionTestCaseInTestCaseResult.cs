using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Trappist.Web.Migrations
{
    public partial class Added_ForeignKeyOfCodeSnippetQuestionTestCaseInTestCaseResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CodeSnippetQuestionTestCasesId",
                table: "TestCaseResult",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeSnippetQuestionTestCasesId",
                table: "TestCaseResult");
        }
    }
}
