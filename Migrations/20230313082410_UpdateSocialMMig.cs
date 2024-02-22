using Microsoft.EntityFrameworkCore.Migrations;

namespace Gameapp.Migrations
{
    public partial class UpdateSocialMMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkedInlink",
                table: "SoicialMidiaLink");

            migrationBuilder.DropColumn(
                name: "facebooklink",
                table: "SoicialMidiaLink");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkedInlink",
                table: "SoicialMidiaLink",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "facebooklink",
                table: "SoicialMidiaLink",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
