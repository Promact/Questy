using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Trappist.Web.Migrations
{
    public partial class AddedTestLogsInTestAttendees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TestLogs_TestAttendeeId",
                table: "TestLogs");

            migrationBuilder.CreateIndex(
                name: "IX_TestLogs_TestAttendeeId",
                table: "TestLogs",
                column: "TestAttendeeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TestLogs_TestAttendeeId",
                table: "TestLogs");

            migrationBuilder.CreateIndex(
                name: "IX_TestLogs_TestAttendeeId",
                table: "TestLogs",
                column: "TestAttendeeId");
        }
    }
}
