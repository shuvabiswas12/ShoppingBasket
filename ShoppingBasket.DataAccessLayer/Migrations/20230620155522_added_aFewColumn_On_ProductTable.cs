using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingBasket.DataAccessLayer.Migrations
{
    public partial class added_aFewColumn_On_ProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessingDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "OrderHeaders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_OrderNumber",
                table: "OrderHeaders",
                column: "OrderNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_OrderNumber",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProcessingDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShippingDate",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
