﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitchenConnection.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class fixitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItem_ShoppingList_ShoppingListId",
                table: "ShoppingListItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShoppingListId",
                table: "ShoppingListItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItem_ShoppingList_ShoppingListId",
                table: "ShoppingListItem",
                column: "ShoppingListId",
                principalTable: "ShoppingList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItem_ShoppingList_ShoppingListId",
                table: "ShoppingListItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShoppingListId",
                table: "ShoppingListItem",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItem_ShoppingList_ShoppingListId",
                table: "ShoppingListItem",
                column: "ShoppingListId",
                principalTable: "ShoppingList",
                principalColumn: "Id");
        }
    }
}
