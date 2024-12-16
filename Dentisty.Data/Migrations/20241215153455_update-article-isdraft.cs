using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatearticleisdraft : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0a778ab6-e941-4b7a-98d3-8383157e4866", "AQAAAAIAAYagAAAAEHwNKpS2+8hB/QMZjsFDyRtijHExq9YEXTdjjNDyoUIwDuee15/+8kUpQ2eMqhlzRA==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "eb6635c7-fc35-47e1-a2db-c13e2f146674", "AQAAAAIAAYagAAAAEBsmmQSdZJ51pn5xJY9kA9Hr5CtkybVYEkBwGPI9h1ZZtmiiI6gAFRMKo3Y9WJQXyg==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "IsDraft" },
                values: new object[] { new DateTime(2024, 12, 15, 22, 34, 54, 159, DateTimeKind.Local).AddTicks(7636), false });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 15, 22, 34, 54, 159, DateTimeKind.Local).AddTicks(7425));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 15, 22, 34, 54, 159, DateTimeKind.Local).AddTicks(7447));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 15, 22, 34, 54, 159, DateTimeKind.Local).AddTicks(7450));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 15, 22, 34, 54, 159, DateTimeKind.Local).AddTicks(7453));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 15, 22, 34, 54, 159, DateTimeKind.Local).AddTicks(7456));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 15, 22, 34, 54, 159, DateTimeKind.Local).AddTicks(7459));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "Articles");

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
                column: "CreatedDate",
                value: new DateTime(2024, 12, 13, 15, 13, 57, 321, DateTimeKind.Local).AddTicks(3881));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 13, 15, 13, 57, 321, DateTimeKind.Local).AddTicks(3892));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 13, 15, 13, 57, 321, DateTimeKind.Local).AddTicks(3894));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 13, 15, 13, 57, 321, DateTimeKind.Local).AddTicks(3895));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 13, 15, 13, 57, 321, DateTimeKind.Local).AddTicks(3897));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 13, 15, 13, 57, 321, DateTimeKind.Local).AddTicks(3899));
        }
    }
}
