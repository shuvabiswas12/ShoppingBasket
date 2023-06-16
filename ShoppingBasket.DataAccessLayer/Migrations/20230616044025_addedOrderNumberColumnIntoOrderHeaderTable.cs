using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingBasket.DataAccessLayer.Migrations
{
    public partial class addedOrderNumberColumnIntoOrderHeaderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "OrderHeaders");
        }
    }
}
