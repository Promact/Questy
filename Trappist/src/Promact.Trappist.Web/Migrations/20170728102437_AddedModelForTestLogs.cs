using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Trappist.Web.Migrations
{
    public partial class AddedModelForTestLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "IX_TestLogs_TestAttendeeId",
                table: "TestLogs",
                column: "TestAttendeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestLogs");
        }
    }
}
