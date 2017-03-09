using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Trappist.Web.Migrations
{
    public partial class Model_Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty_Level",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "Question");

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Question",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DifficultyLevel",
                table: "Question",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Question",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Question",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDateTime",
                table: "Question",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDateTime",
                table: "Options",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Options",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "DifficultyLevel",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Question");

            migrationBuilder.AddColumn<string>(
                name: "Difficulty_Level",
                table: "Question",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "category_id",
                table: "Question",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDateTime",
                table: "Question",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDateTime",
                table: "Options",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Options",
                nullable: true);
        }
    }
}
