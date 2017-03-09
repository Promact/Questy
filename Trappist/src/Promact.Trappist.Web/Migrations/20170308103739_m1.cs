using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Trappist.Web.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Answer = table.Column<bool>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Question_Id = table.Column<int>(nullable: true),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Options_Question_Question_Id",
                        column: x => x.Question_Id,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<string>(
                name: "Difficulty_Level",
                table: "Question",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Question",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Question",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "category_id",
                table: "Question",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Question",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Options_Question_Id",
                table: "Options",
                column: "Question_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty_Level",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "Question");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Question",
                nullable: true);
        }
    }
}
