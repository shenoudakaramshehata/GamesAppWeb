using Microsoft.EntityFrameworkCore.Migrations;

namespace Gameapp.Migrations
{
    public partial class AddScoreAttr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "ChampionParticipates",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "ChampionParticipates");
        }
    }
}
