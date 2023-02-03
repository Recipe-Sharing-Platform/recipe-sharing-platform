using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitchenConnection.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class deletedshoppingList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItem_ShoppingList_ShoppingListId",
                table: "ShoppingListItem");

            migrationBuilder.DropTable(
                name: "ShoppingList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingListItem",
                table: "ShoppingListItem");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "ShoppingListItem");

            migrationBuilder.RenameColumn(
                name: "ShoppingListId",
                table: "ShoppingListItem",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingListItem_ShoppingListId",
                table: "ShoppingListItem",
                newName: "IX_ShoppingListItem_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ShoppingListItem",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingListItem",
                table: "ShoppingListItem",
                columns: new[] { "Id", "UserId", "Name" });

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItem_Users_UserId",
                table: "ShoppingListItem",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItem_Users_UserId",
                table: "ShoppingListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingListItem",
                table: "ShoppingListItem");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ShoppingListItem",
                newName: "ShoppingListId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingListItem_UserId",
                table: "ShoppingListItem",
                newName: "IX_ShoppingListItem_ShoppingListId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ShoppingListItem",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "ShoppingListItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingListItem",
                table: "ShoppingListItem",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ShoppingList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingList_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingList_UserId",
                table: "ShoppingList",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItem_ShoppingList_ShoppingListId",
                table: "ShoppingListItem",
                column: "ShoppingListId",
                principalTable: "ShoppingList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
