using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Trappist.Web.Migrations
{
    public partial class TestIpAddressAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromIpAddress",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "ToIpAddress",
                table: "Test");

            migrationBuilder.CreateTable(
                name: "TestIPAddress",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    IPAddress = table.Column<string>(nullable: true),
                    TestId = table.Column<int>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestIPAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestIPAddress_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestIPAddress_TestId",
                table: "TestIPAddress",
                column: "TestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestIPAddress");

            migrationBuilder.AddColumn<string>(
                name: "FromIpAddress",
                table: "Test",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToIpAddress",
                table: "Test",
                nullable: true);
        }
    }
}
