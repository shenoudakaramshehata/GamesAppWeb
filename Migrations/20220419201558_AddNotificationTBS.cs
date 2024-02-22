using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gameapp.Migrations
{
    public partial class AddNotificationTBS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubCategoryPic",
                table: "SubCategory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Pic",
                table: "Country",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "PublicDevices",
                columns: table => new
                {
                    PublicDeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAndroiodDevice = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicDevices", x => x.PublicDeviceId);
                    table.ForeignKey(
                        name: "FK_PublicDevices_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublicNotifications",
                columns: table => new
                {
                    PublicNotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SliderTypeId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicNotifications", x => x.PublicNotificationId);
                    table.ForeignKey(
                        name: "FK_PublicNotifications_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicNotifications_SliderType_SliderTypeId",
                        column: x => x.SliderTypeId,
                        principalTable: "SliderType",
                        principalColumn: "SliderTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublicNotificationDevices",
                columns: table => new
                {
                    PublicNotificationDeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicNotificationId = table.Column<int>(type: "int", nullable: false),
                    PublicDeviceId = table.Column<int>(type: "int", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicNotificationDevices", x => x.PublicNotificationDeviceId);
                    table.ForeignKey(
                        name: "FK_PublicNotificationDevices_PublicDevices_PublicDeviceId",
                        column: x => x.PublicDeviceId,
                        principalTable: "PublicDevices",
                        principalColumn: "PublicDeviceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublicNotificationDevices_PublicNotifications_PublicNotificationId",
                        column: x => x.PublicNotificationId,
                        principalTable: "PublicNotifications",
                        principalColumn: "PublicNotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicDevices_CountryId",
                table: "PublicDevices",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotificationDevices_PublicDeviceId",
                table: "PublicNotificationDevices",
                column: "PublicDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotificationDevices_PublicNotificationId",
                table: "PublicNotificationDevices",
                column: "PublicNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotifications_CountryId",
                table: "PublicNotifications",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotifications_SliderTypeId",
                table: "PublicNotifications",
                column: "SliderTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicNotificationDevices");

            migrationBuilder.DropTable(
                name: "PublicDevices");

            migrationBuilder.DropTable(
                name: "PublicNotifications");

            migrationBuilder.AlterColumn<string>(
                name: "SubCategoryPic",
                table: "SubCategory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Pic",
                table: "Country",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
