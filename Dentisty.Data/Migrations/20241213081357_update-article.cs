using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatearticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1e73a657-62ab-45a6-b7d4-7f673c13b828", "AQAAAAIAAYagAAAAEHTToiweyjSbuWtvbkjgq7pWmO2vbO3y+YZtyk6GphnTmC/8GBlRtxbPUZKTxYI/Ow==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5d2c0f66-570c-437d-8093-ee78c79f3fa2", "AQAAAAIAAYagAAAAEM9dTowlCMyYXUuAfKCrQ5CYfL0qFCn93sy/OONunp9jR0g/EgrC9wAVe5DpGl3HSw==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 13, 15, 13, 57, 321, DateTimeKind.Local).AddTicks(3940));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Description", "ImageId", "Sort" },
                values: new object[] { new DateTime(2024, 12, 13, 15, 13, 57, 321, DateTimeKind.Local).AddTicks(3881), null, null, 1 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Description", "ImageId", "Sort" },
                values: new object[] { new DateTime(2024, 12, 13, 15, 13, 57, 321, DateTimeKind.Local).AddTicks(3892), null, null, 2 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Description", "ImageId", "Sort" },
                values: new object[] { new DateTime(2024, 12, 13, 15, 13, 57, 321, DateTimeKind.Local).AddTicks(3894), null, null, 3 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "Description", "ImageId", "Sort" },
                values: new object[] { new DateTime(2024, 12, 13, 15, 13, 57, 321, DateTimeKind.Local).AddTicks(3895), null, null, 4 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "Description", "ImageId", "Sort" },
                values: new object[] { new DateTime(2024, 12, 13, 15, 13, 57, 321, DateTimeKind.Local).AddTicks(3897), null, null, 5 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "Description", "ImageId", "Sort" },
                values: new object[] { new DateTime(2024, 12, 13, 15, 13, 57, 321, DateTimeKind.Local).AddTicks(3899), null, null, 6 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Articles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
                columns: new[] { "CreatedDate", "Sort" },
                values: new object[] { new DateTime(2024, 12, 12, 12, 39, 10, 107, DateTimeKind.Local).AddTicks(1481), 0 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Sort" },
                values: new object[] { new DateTime(2024, 12, 12, 12, 39, 10, 107, DateTimeKind.Local).AddTicks(1495), 0 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Sort" },
                values: new object[] { new DateTime(2024, 12, 12, 12, 39, 10, 107, DateTimeKind.Local).AddTicks(1497), 0 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "Sort" },
                values: new object[] { new DateTime(2024, 12, 12, 12, 39, 10, 107, DateTimeKind.Local).AddTicks(1498), 0 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "Sort" },
                values: new object[] { new DateTime(2024, 12, 12, 12, 39, 10, 107, DateTimeKind.Local).AddTicks(1500), 0 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "Sort" },
                values: new object[] { new DateTime(2024, 12, 12, 12, 39, 10, 107, DateTimeKind.Local).AddTicks(1502), 0 });
        }
    }
}
