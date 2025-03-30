using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class addtimezone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdLogin",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "IdLogin", "PasswordHash", "TimeZone" },
                values: new object[] { "834bd250-8252-491b-8b33-88b90ac12360", null, "AQAAAAIAAYagAAAAEF7/9e7hq4pZAitwawQLft/hg0NMTt3GZ7jY5ba9PGzFfgq+FA0FoJAtEjMP9CIiAg==", null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(4971));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(4990));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(4993));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(4995));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(4997));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(4999));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(5001));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdLogin",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "AppUsers");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e5f6fe88-3cc6-4688-81c0-a6dd4c939948", "AQAAAAIAAYagAAAAEHDZAdqnaNjr4+jp0SjYQYecOOQ83NmtQTHcDDHMlDx1PfUVZ5yNWHwJ6BLYquAg9g==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 18, 15, 48, 15, 89, DateTimeKind.Local).AddTicks(6106));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 18, 15, 48, 15, 89, DateTimeKind.Local).AddTicks(6125));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 18, 15, 48, 15, 89, DateTimeKind.Local).AddTicks(6128));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 18, 15, 48, 15, 89, DateTimeKind.Local).AddTicks(6130));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 18, 15, 48, 15, 89, DateTimeKind.Local).AddTicks(6133));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 18, 15, 48, 15, 89, DateTimeKind.Local).AddTicks(6135));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 18, 15, 48, 15, 89, DateTimeKind.Local).AddTicks(6137));
        }
    }
}
