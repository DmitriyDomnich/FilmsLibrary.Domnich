using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class AddListOrderTableWithConnections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_ListOrders_ListOrderId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ListOrders_ListOrderId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ListOrderId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Movies_ListOrderId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ListOrderId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ListOrderId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ListOrdersId",
                table: "Movies");

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "ListOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ListOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ListOrders_MovieId",
                table: "ListOrders",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_ListOrders_OrderId",
                table: "ListOrders",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListOrders_Movies_MovieId",
                table: "ListOrders",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListOrders_Orders_OrderId",
                table: "ListOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListOrders_Movies_MovieId",
                table: "ListOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ListOrders_Orders_OrderId",
                table: "ListOrders");

            migrationBuilder.DropIndex(
                name: "IX_ListOrders_MovieId",
                table: "ListOrders");

            migrationBuilder.DropIndex(
                name: "IX_ListOrders_OrderId",
                table: "ListOrders");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "ListOrders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ListOrders");

            migrationBuilder.AddColumn<int>(
                name: "ListOrderId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ListOrderId",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ListOrdersId",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ListOrderId",
                table: "Orders",
                column: "ListOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ListOrderId",
                table: "Movies",
                column: "ListOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_ListOrders_ListOrderId",
                table: "Movies",
                column: "ListOrderId",
                principalTable: "ListOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ListOrders_ListOrderId",
                table: "Orders",
                column: "ListOrderId",
                principalTable: "ListOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
