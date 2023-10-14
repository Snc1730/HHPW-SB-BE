using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HHPW_SB_BE.Migrations
{
    public partial class AddIsEmployeePropertyToEmployeeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isEmployee",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "isEmployee",
                value: true);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateClosed", "DatePlaced" },
                values: new object[] { new DateTime(2023, 10, 14, 18, 40, 32, 501, DateTimeKind.Local).AddTicks(8160), new DateTime(2023, 10, 13, 18, 40, 32, 501, DateTimeKind.Local).AddTicks(8112) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isEmployee",
                table: "Employees");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateClosed", "DatePlaced" },
                values: new object[] { new DateTime(2023, 10, 10, 17, 26, 21, 38, DateTimeKind.Local).AddTicks(8104), new DateTime(2023, 10, 9, 17, 26, 21, 38, DateTimeKind.Local).AddTicks(8066) });
        }
    }
}
