using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionsWHs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Products_ProductId1",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ProductId1",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SourceWHId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TargetWHId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SourceWarehouse",
                table: "Transactions",
                newName: "TargetWarehouseId");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceWarehouseId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceWarehouseWarehouseId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SourceWarehouseWarehouseId",
                table: "Transactions",
                column: "SourceWarehouseWarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Warehouses_SourceWarehouseWarehouseId",
                table: "Transactions",
                column: "SourceWarehouseWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Warehouses_SourceWarehouseWarehouseId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SourceWarehouseWarehouseId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseWarehouseId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TargetWarehouseId",
                table: "Transactions",
                newName: "SourceWarehouse");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "SourceWHId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TargetWHId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ProductId1",
                table: "Transactions",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Products_ProductId1",
                table: "Transactions",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
