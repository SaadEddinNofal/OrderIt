using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderITDemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial_End : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_CartsItem_CartItemId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CartItemId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "CartItemId",
                table: "Invoices",
                newName: "Quentity");

            migrationBuilder.AddColumn<string>(
                name: "MenuName",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuName",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "Quentity",
                table: "Invoices",
                newName: "CartItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CartItemId",
                table: "Invoices",
                column: "CartItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_CartsItem_CartItemId",
                table: "Invoices",
                column: "CartItemId",
                principalTable: "CartsItem",
                principalColumn: "CartItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
