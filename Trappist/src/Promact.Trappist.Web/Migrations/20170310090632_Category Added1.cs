using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Trappist.Web.Migrations
{
    public partial class CategoryAdded1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleMultipleAnswerQuestionOption_QuestionBase_SingleMultipleAnswerQuestionID",
                table: "SingleMultipleAnswerQuestionOption");

            migrationBuilder.DropTable(
                name: "QuestionBase");

            migrationBuilder.CreateTable(
                name: "SingleMultipleAnswerQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryID = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    DifficultyLevel = table.Column<int>(nullable: false),
                    QuestionDetail = table.Column<string>(nullable: false),
                    QuestionType = table.Column<int>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleMultipleAnswerQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SingleMultipleAnswerQuestion_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SingleMultipleAnswerQuestion_CategoryID",
                table: "SingleMultipleAnswerQuestion",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleMultipleAnswerQuestionOption_SingleMultipleAnswerQuestion_SingleMultipleAnswerQuestionID",
                table: "SingleMultipleAnswerQuestionOption",
                column: "SingleMultipleAnswerQuestionID",
                principalTable: "SingleMultipleAnswerQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleMultipleAnswerQuestionOption_SingleMultipleAnswerQuestion_SingleMultipleAnswerQuestionID",
                table: "SingleMultipleAnswerQuestionOption");

            migrationBuilder.DropTable(
                name: "SingleMultipleAnswerQuestion");

            migrationBuilder.CreateTable(
                name: "QuestionBase",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryID = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    DifficultyLevel = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    QuestionDetail = table.Column<string>(nullable: false),
                    QuestionType = table.Column<int>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionBase_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionBase_CategoryID",
                table: "QuestionBase",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleMultipleAnswerQuestionOption_QuestionBase_SingleMultipleAnswerQuestionID",
                table: "SingleMultipleAnswerQuestionOption",
                column: "SingleMultipleAnswerQuestionID",
                principalTable: "QuestionBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
