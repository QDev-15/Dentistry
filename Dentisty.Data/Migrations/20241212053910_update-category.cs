using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatecategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ff6e0353-a5ef-4493-9f58-b520b74be7a9", "AQAAAAIAAYagAAAAEOQUaQo2funXcEGgNsvsUXMg3TxpiAgPOdIi3eIE8x9OYLL/fb0zAlz0quI0bgvhcA==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "14dc25c5-295e-43f2-a216-83f797cf6280", "AQAAAAIAAYagAAAAEF1XBKC0UwsU6+a45SVaNigxbnmVDqYpEWSFHVywNigF4peKUN7BMnPZTKL/baLNuQ==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 12, 12, 39, 10, 107, DateTimeKind.Local).AddTicks(1546));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 12, 12, 39, 10, 107, DateTimeKind.Local).AddTicks(1481));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 12, 12, 39, 10, 107, DateTimeKind.Local).AddTicks(1495));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 12, 12, 39, 10, 107, DateTimeKind.Local).AddTicks(1497));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 12, 12, 39, 10, 107, DateTimeKind.Local).AddTicks(1498));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 12, 12, 39, 10, 107, DateTimeKind.Local).AddTicks(1500));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 12, 12, 39, 10, 107, DateTimeKind.Local).AddTicks(1502));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sort",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3c6af3d9-2c0d-4e95-b663-45cd487e0eb0", "AQAAAAIAAYagAAAAEHeixQwtmXqT98bY8eCK+wpIJBuSfEUG9A4mbyWXBrmR/rMe8JyrNfsOSN+JJIXsVg==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6f9a6f45-7abc-482b-99fa-4b2fc4db76ac", "AQAAAAIAAYagAAAAEH8awW1B5d/HQmNhFJbnr1e0DAKN4FLJdn+ItrddJABR07rzALK9eU3NhIsiWpD3lA==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 10, 23, 15, 20, 539, DateTimeKind.Local).AddTicks(8891));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 10, 23, 15, 20, 539, DateTimeKind.Local).AddTicks(8664));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 10, 23, 15, 20, 539, DateTimeKind.Local).AddTicks(8686));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 10, 23, 15, 20, 539, DateTimeKind.Local).AddTicks(8688));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 10, 23, 15, 20, 539, DateTimeKind.Local).AddTicks(8690));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 10, 23, 15, 20, 539, DateTimeKind.Local).AddTicks(8693));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 10, 23, 15, 20, 539, DateTimeKind.Local).AddTicks(8696));
        }
    }
}
