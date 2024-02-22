using Microsoft.EntityFrameworkCore.Migrations;

namespace Gameapp.Migrations
{
    public partial class ChampionTyMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ChampionType",
                newName: "NameEn");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "ChampionType",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "ChampionType");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "ChampionType",
                newName: "Name");
        }
    }
}
