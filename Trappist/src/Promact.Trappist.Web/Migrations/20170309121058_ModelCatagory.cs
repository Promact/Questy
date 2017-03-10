using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Trappist.Web.Migrations
{
    public partial class ModelCatagory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_categorys",
                table: "categorys");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "categorys",
                column: "Id");

            migrationBuilder.RenameColumn(
                name: "categoryName",
                table: "categorys",
                newName: "CategoryName");

            migrationBuilder.RenameTable(
                name: "categorys",
                newName: "Category");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categorys",
                table: "Category",
                column: "Id");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Category",
                newName: "categoryName");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "categorys");
        }
    }
}
