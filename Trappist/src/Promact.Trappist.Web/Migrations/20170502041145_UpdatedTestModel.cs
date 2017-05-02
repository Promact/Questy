using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Trappist.Web.Migrations
{
    public partial class UpdatedTestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Test",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Test_CreatedByUserId",
                table: "Test",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Test_AspNetUsers_CreatedByUserId",
                table: "Test",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Test_AspNetUsers_CreatedByUserId",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Test_CreatedByUserId",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Test");
        }
    }
}
