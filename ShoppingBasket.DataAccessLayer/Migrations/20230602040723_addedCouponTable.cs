using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingBasket.DataAccessLayer.Migrations
{
    public partial class addedCouponTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CouponCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    CouponName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CouponRate = table.Column<double>(type: "float", maxLength: 70, nullable: false),
                    MinOrderedAmount = table.Column<double>(type: "float", nullable: false),
                    MaxOrderedAmount = table.Column<double>(type: "float", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
