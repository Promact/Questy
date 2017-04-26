using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Trappist.Web.Migrations
{
    public partial class TestConductModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BlockedTest",
                table: "TestAttendees",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CompletedTest",
                table: "TestAttendees",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExpiredTest",
                table: "TestAttendees",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StarredCandidate",
                table: "TestAttendees",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockedTest",
                table: "TestAttendees");

            migrationBuilder.DropColumn(
                name: "CompletedTest",
                table: "TestAttendees");

            migrationBuilder.DropColumn(
                name: "ExpiredTest",
                table: "TestAttendees");

            migrationBuilder.DropColumn(
                name: "StarredCandidate",
                table: "TestAttendees");
        }
    }
}
