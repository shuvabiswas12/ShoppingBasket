using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingBasket.DataAccessLayer.Migrations
{
    public partial class addedUserIdColumnOnWishListTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Wishlists",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_ApplicationUserId",
                table: "Wishlists",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_AspNetUsers_ApplicationUserId",
                table: "Wishlists",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_AspNetUsers_ApplicationUserId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_ApplicationUserId",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Wishlists");
        }
    }
}
