using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitchenConnection.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class test4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_CookBook_CookBookId",
                table: "Recipes");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_CookBook_CookBookId",
                table: "Recipes",
                column: "CookBookId",
                principalTable: "CookBook",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_CookBook_CookBookId",
                table: "Recipes");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_CookBook_CookBookId",
                table: "Recipes",
                column: "CookBookId",
                principalTable: "CookBook",
                principalColumn: "Id");
        }
    }
}
