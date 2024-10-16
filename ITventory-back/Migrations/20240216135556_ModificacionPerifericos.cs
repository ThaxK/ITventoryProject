using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Itventory.web.Migrations
{
    /// <inheritdoc />
    public partial class ModificacionPerifericos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Peripherals",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Peripherals",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Peripherals");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Peripherals");
        }
    }
}
