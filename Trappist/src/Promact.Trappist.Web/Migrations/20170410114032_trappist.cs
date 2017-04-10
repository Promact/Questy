using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Trappist.Web.Migrations
{
    public partial class trappist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestAttendees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContactNumber = table.Column<string>(maxLength: 15, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(maxLength: 255, nullable: false),
                    LastName = table.Column<string>(maxLength: 255, nullable: false),
                    RollNumber = table.Column<string>(nullable: false),
                    TestID = table.Column<int>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAttendees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestAttendees_Test_TestID",
                        column: x => x.TestID,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestAttendees_TestID",
                table: "TestAttendees",
                column: "TestID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestAttendees");
        }
    }
}
