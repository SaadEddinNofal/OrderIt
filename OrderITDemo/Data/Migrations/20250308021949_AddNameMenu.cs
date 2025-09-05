using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderITDemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNameMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MenuName",
                table: "CartsItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuName",
                table: "CartsItem");
        }
    }
}
