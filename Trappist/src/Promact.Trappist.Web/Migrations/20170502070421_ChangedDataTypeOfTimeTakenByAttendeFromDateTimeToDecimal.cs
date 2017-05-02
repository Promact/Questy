using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Trappist.Web.Migrations
{
    public partial class ChangedDataTypeOfTimeTakenByAttendeFromDateTimeToDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeTakenByTheAttendee",
                table: "Report");

            migrationBuilder.AddColumn<decimal>(
                name: "TimeTakenByAttendee",
                table: "Report",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeTakenByAttendee",
                table: "Report");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeTakenByTheAttendee",
                table: "Report",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
