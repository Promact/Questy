using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Trappist.Web.Migrations
{
    public partial class AddedAllowTestResumeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WarningTime",
                table: "Test",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "AllowTestResume",
                table: "Test",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowTestResume",
                table: "Test");

            migrationBuilder.AlterColumn<int>(
                name: "WarningTime",
                table: "Test",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
