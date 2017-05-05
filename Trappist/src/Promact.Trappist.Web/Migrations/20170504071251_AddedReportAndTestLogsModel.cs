using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Trappist.Web.Migrations
{
    public partial class AddedReportAndTestLogsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Percentage = table.Column<double>(nullable: false),
                    Percentile = table.Column<double>(nullable: false),
                    TestAttendeeId = table.Column<int>(nullable: false),
                    TestStatus = table.Column<int>(nullable: false),
                    TimeTakenByAttendee = table.Column<int>(nullable: false),
                    TotalMarksScored = table.Column<double>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Report_TestAttendees_TestAttendeeId",
                        column: x => x.TestAttendeeId,
                        principalTable: "TestAttendees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AwayFromTestWindow = table.Column<DateTime>(nullable: true),
                    CloseWindowWithoutFinishingTest = table.Column<DateTime>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    DisconnectedFromServer = table.Column<DateTime>(nullable: true),
                    FillRegistrationForm = table.Column<DateTime>(nullable: false),
                    FinishTest = table.Column<DateTime>(nullable: false),
                    PassInstructionpage = table.Column<DateTime>(nullable: false),
                    ResumeTest = table.Column<DateTime>(nullable: true),
                    StartTest = table.Column<DateTime>(nullable: false),
                    TestAttendeeId = table.Column<int>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true),
                    VisitTestLink = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestLogs_TestAttendees_TestAttendeeId",
                        column: x => x.TestAttendeeId,
                        principalTable: "TestAttendees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Report_TestAttendeeId",
                table: "Report",
                column: "TestAttendeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestLogs_TestAttendeeId",
                table: "TestLogs",
                column: "TestAttendeeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "TestLogs");

            migrationBuilder.AddColumn<int>(
                name: "TestState",
                table: "TestAttendees",
                nullable: false,
                defaultValue: 0);
        }
    }
}
