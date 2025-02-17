using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManager.DataAccess.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDate",
                table: "Orders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DateOfBirth",
                table: "AspNetUsers",
                column: "DateOfBirth");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderDate",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DateOfBirth",
                table: "AspNetUsers");
        }
    }
}
