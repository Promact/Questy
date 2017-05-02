using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Trappist.Web.Migrations
{
    public partial class UpdatedTestAttendeeModelAndReportModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestStatus",
                table: "TestAttendees");

            migrationBuilder.AddColumn<int>(
                name: "TestStatus",
                table: "Report",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeTakenByTheAttendee",
                table: "Report",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestStatus",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "TimeTakenByTheAttendee",
                table: "Report");

            migrationBuilder.AddColumn<int>(
                name: "TestStatus",
                table: "TestAttendees",
                nullable: false,
                defaultValue: 0);
        }
    }
}
