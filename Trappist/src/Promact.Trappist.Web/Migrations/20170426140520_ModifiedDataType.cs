using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Trappist.Web.Migrations
{
    public partial class ModifiedDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WarningTime",
                table: "Test",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WarningTime",
                table: "Test",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
