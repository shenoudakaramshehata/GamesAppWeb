﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Gameapp.Migrations
{
    public partial class fix2payment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Shop_ShopId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "ShopId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "uniqeId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Shop_ShopId",
                table: "Order",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Shop_ShopId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "uniqeId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "ShopId",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Shop_ShopId",
                table: "Order",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
