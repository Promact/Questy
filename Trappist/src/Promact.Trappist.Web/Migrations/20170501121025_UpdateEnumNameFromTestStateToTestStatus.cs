using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Trappist.Web.Migrations
{
    public partial class UpdateEnumNameFromTestStateToTestStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestState",
                table: "TestAttendees");

            migrationBuilder.AddColumn<int>(
                name: "TestStatus",
                table: "TestAttendees",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestStatus",
                table: "TestAttendees");

            migrationBuilder.AddColumn<int>(
                name: "TestState",
                table: "TestAttendees",
                nullable: false,
                defaultValue: 0);
        }
    }
}
