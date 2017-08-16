using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Trappist.Web.Migrations
{
    public partial class AddedTestCaseResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestCaseResult",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Memory = table.Column<long>(nullable: false),
                    Output = table.Column<string>(nullable: true),
                    Processing = table.Column<long>(nullable: false),
                    TestCodeSolutionId = table.Column<int>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCaseResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCaseResult_TestCodeSolution_TestCodeSolutionId",
                        column: x => x.TestCodeSolutionId,
                        principalTable: "TestCodeSolution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestCaseResult_TestCodeSolutionId",
                table: "TestCaseResult",
                column: "TestCodeSolutionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestCaseResult");
        }
    }
}
