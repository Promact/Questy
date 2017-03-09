using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Trappist.Web.Migrations
{
    public partial class migrationcsq1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodeSnippetQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CheckCodeComplexity = table.Column<bool>(nullable: false),
                    CheckTimeComplexity = table.Column<bool>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    RunBasicTestCase = table.Column<bool>(nullable: false),
                    RunCornerTestCase = table.Column<bool>(nullable: false),
                    RunNecessaryTestCase = table.Column<bool>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeSnippetQuestion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodingLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Language = table.Column<string>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodingLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionLanguageMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionLanguageMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionLanguageMapping_CodingLanguage_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "CodingLanguage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionLanguageMapping_CodeSnippetQuestion_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "CodeSnippetQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionLanguageMapping_LanguageId",
                table: "QuestionLanguageMapping",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionLanguageMapping_QuestionId",
                table: "QuestionLanguageMapping",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionLanguageMapping");

            migrationBuilder.DropTable(
                name: "CodingLanguage");

            migrationBuilder.DropTable(
                name: "CodeSnippetQuestion");
        }
    }
}
