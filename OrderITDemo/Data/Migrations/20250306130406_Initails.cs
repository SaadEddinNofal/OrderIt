using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderITDemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartsItem_Menus_MenuItemId1",
                table: "CartsItem");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "CartsItem");

            migrationBuilder.DropColumn(
                name: "MenuItemId",
                table: "CartsItem");

            migrationBuilder.RenameColumn(
                name: "MenuItemId1",
                table: "CartsItem",
                newName: "MenuId");

            migrationBuilder.RenameIndex(
                name: "IX_CartsItem_MenuItemId1",
                table: "CartsItem",
                newName: "IX_CartsItem_MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartsItem_Menus_MenuId",
                table: "CartsItem",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "MenuItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartsItem_Menus_MenuId",
                table: "CartsItem");

            migrationBuilder.RenameColumn(
                name: "MenuId",
                table: "CartsItem",
                newName: "MenuItemId1");

            migrationBuilder.RenameIndex(
                name: "IX_CartsItem_MenuId",
                table: "CartsItem",
                newName: "IX_CartsItem_MenuItemId1");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "CartsItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MenuItemId",
                table: "CartsItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CartsItem_Menus_MenuItemId1",
                table: "CartsItem",
                column: "MenuItemId1",
                principalTable: "Menus",
                principalColumn: "MenuItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
