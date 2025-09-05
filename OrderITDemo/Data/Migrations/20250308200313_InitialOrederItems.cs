using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderITDemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialOrederItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Invoices",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Invoices",
                newName: "TotalAmount");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
