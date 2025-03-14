using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueenOfApostlesRenewalCentre.Migrations
{
    /// <inheritdoc />
    public partial class RobustInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Invoices",
                newName: "TotalAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "BreakfastCost",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DinnerCost",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DirectorsDiscount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Invoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "LunchCost",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PremisesUseCost",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RoomCost",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakfastCost",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DinnerCost",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DirectorsDiscount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "LunchCost",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "PremisesUseCost",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RoomCost",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Invoices",
                newName: "Amount");
        }
    }
}
