﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCartApp.Migrations
{
    /// <inheritdoc />
    public partial class FixedForeignKeyIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseItems_PurchaseHistories_PurchaseHistoryId",
                table: "PurchaseItems");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "PurchaseItems");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseHistoryId",
                table: "PurchaseItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseItems_PurchaseHistories_PurchaseHistoryId",
                table: "PurchaseItems",
                column: "PurchaseHistoryId",
                principalTable: "PurchaseHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseItems_PurchaseHistories_PurchaseHistoryId",
                table: "PurchaseItems");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseHistoryId",
                table: "PurchaseItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "PurchaseItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseItems_PurchaseHistories_PurchaseHistoryId",
                table: "PurchaseItems",
                column: "PurchaseHistoryId",
                principalTable: "PurchaseHistories",
                principalColumn: "Id");
        }
    }
}
