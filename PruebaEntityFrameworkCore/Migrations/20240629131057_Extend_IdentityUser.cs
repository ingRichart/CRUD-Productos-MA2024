using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PruebaEntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class Extend_IdentityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HelpPassword",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QuestionOne",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QuiestionTwo",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HelpPassword",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "QuestionOne",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "QuiestionTwo",
                table: "AspNetUsers");
        }
    }
}
