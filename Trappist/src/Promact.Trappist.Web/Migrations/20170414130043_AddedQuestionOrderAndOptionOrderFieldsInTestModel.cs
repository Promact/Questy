using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Trappist.Web.Migrations
{
    public partial class AddedQuestionOrderAndOptionOrderFieldsInTestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OptionOrder",
                table: "Test",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuestionOrder",
                table: "Test",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionOrder",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "QuestionOrder",
                table: "Test");
        }
    }
}
