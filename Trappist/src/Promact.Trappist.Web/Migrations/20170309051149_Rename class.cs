using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Trappist.Web.Migrations
{
    public partial class Renameclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Question_Question_Id",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Options");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.CreateTable(
                name: "SingleMultipleAnswerQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryID = table.Column<int>(nullable: false),
                    CreateBy = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    DifficultyLevel = table.Column<int>(nullable: false),
                    QuestionDetail = table.Column<string>(nullable: false),
                    QuestionType = table.Column<int>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleMultipleAnswerQuestion", x => x.Id);
                });

            migrationBuilder.AddColumn<bool>(
                name: "IsAnswer",
                table: "Options",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SingleMultipleAnswerOption",
                table: "Options",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_SingleMultipleAnswerQuestion_Question_Id",
                table: "Options",
                column: "Question_Id",
                principalTable: "SingleMultipleAnswerQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_SingleMultipleAnswerQuestion_Question_Id",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "IsAnswer",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "SingleMultipleAnswerOption",
                table: "Options");

            migrationBuilder.DropTable(
                name: "SingleMultipleAnswerQuestion");

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryID = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    DifficultyLevel = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                });

            migrationBuilder.AddColumn<bool>(
                name: "Answer",
                table: "Options",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Options",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Question_Question_Id",
                table: "Options",
                column: "Question_Id",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
