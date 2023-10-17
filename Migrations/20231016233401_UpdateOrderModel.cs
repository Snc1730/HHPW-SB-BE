using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HHPW_SB_BE.Migrations
{
    public partial class UpdateOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MenuItemQuantities",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateClosed", "DatePlaced", "MenuItemQuantities" },
                values: new object[] { new DateTime(2023, 10, 17, 19, 34, 1, 405, DateTimeKind.Local).AddTicks(5849), new DateTime(2023, 10, 16, 19, 34, 1, 405, DateTimeKind.Local).AddTicks(5806), "{\"1\":2,\"2\":3}" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuItemQuantities",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateClosed", "DatePlaced" },
                values: new object[] { new DateTime(2023, 10, 14, 18, 40, 32, 501, DateTimeKind.Local).AddTicks(8160), new DateTime(2023, 10, 13, 18, 40, 32, 501, DateTimeKind.Local).AddTicks(8112) });
        }
    }
}
