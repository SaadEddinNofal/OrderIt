using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderITDemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update_Id_Invoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Invoices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
