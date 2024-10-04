using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class AddWHNameTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Warehouses_SourceWarehouseWarehouseId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Warehouses_TargetWarehouseWarehouseId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SourceWarehouseWarehouseId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TargetWarehouseWarehouseId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseWarehouseId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseWarehouseId",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "SourceWarehouseName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetWarehouseName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceWarehouseName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseName",
                table: "Transactions");

            migrationBuilder.AddColumn<Guid>(
                name: "SourceWarehouseWarehouseId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TargetWarehouseWarehouseId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SourceWarehouseWarehouseId",
                table: "Transactions",
                column: "SourceWarehouseWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TargetWarehouseWarehouseId",
                table: "Transactions",
                column: "TargetWarehouseWarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Warehouses_SourceWarehouseWarehouseId",
                table: "Transactions",
                column: "SourceWarehouseWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Warehouses_TargetWarehouseWarehouseId",
                table: "Transactions",
                column: "TargetWarehouseWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId");
        }
    }
}
