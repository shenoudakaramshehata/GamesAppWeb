using Microsoft.EntityFrameworkCore.Migrations;

namespace Gameapp.Migrations
{
    public partial class Banner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Banner_SliderTypeId",
                table: "Banner",
                column: "SliderTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Banner_SliderType_SliderTypeId",
                table: "Banner",
                column: "SliderTypeId",
                principalTable: "SliderType",
                principalColumn: "SliderTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banner_SliderType_SliderTypeId",
                table: "Banner");

            migrationBuilder.DropIndex(
                name: "IX_Banner_SliderTypeId",
                table: "Banner");
        }
    }
}
