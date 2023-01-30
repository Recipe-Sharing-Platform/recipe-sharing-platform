using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitchenConnection.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class fixedCollectionOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionRecipes_Collections_CollectionId",
                table: "CollectionRecipes");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionRecipes_Collections_CollectionId",
                table: "CollectionRecipes",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionRecipes_Collections_CollectionId",
                table: "CollectionRecipes");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionRecipes_Collections_CollectionId",
                table: "CollectionRecipes",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id");
        }
    }
}
