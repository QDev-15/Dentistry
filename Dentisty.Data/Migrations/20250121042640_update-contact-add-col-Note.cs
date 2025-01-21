using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatecontactaddcolNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "04ed7eec-c516-42ae-a5f1-c36f9206101e", "AQAAAAIAAYagAAAAEGXqH9M3K0UDIV0G8nPWt0E/Rvw3x7M9HkiylwAUCuhpSmzM2UoWzdH/zyGHJC8yaQ==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "95e18b37-0de8-405b-9b63-9868661be938", "AQAAAAIAAYagAAAAEOQCjGcN7Lg++V8JGWW703cxwADWI109tct+KR2pABO7zaieMaLrQ00FBT27vLdf0Q==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 21, 11, 26, 40, 178, DateTimeKind.Local).AddTicks(4871));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 21, 11, 26, 40, 178, DateTimeKind.Local).AddTicks(4790));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 21, 11, 26, 40, 178, DateTimeKind.Local).AddTicks(4804));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 21, 11, 26, 40, 178, DateTimeKind.Local).AddTicks(4806));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 21, 11, 26, 40, 178, DateTimeKind.Local).AddTicks(4807));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 21, 11, 26, 40, 178, DateTimeKind.Local).AddTicks(4809));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 21, 11, 26, 40, 178, DateTimeKind.Local).AddTicks(4810));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Contacts");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9ebf31ad-a66a-40c7-af1f-fef7d7e13f23", "AQAAAAIAAYagAAAAEMiSgXH2G9P3+XaMxL1Av1QUQyNxNqTDuAH4Z15N42n/IR0/9GMnosPLUzikOKmOUg==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9d307c69-1771-4a22-a89f-dc93e488c17d", "AQAAAAIAAYagAAAAENrJSOxmNIJLCM02QN/ltj+VZAsXT5GD5o4Bl7qc5VebViriEmg5sEBiaSbYMP9GKQ==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 20, 9, 52, 42, 119, DateTimeKind.Local).AddTicks(1289));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 20, 9, 52, 42, 119, DateTimeKind.Local).AddTicks(1225));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 20, 9, 52, 42, 119, DateTimeKind.Local).AddTicks(1239));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 20, 9, 52, 42, 119, DateTimeKind.Local).AddTicks(1241));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 20, 9, 52, 42, 119, DateTimeKind.Local).AddTicks(1243));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 20, 9, 52, 42, 119, DateTimeKind.Local).AddTicks(1244));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 20, 9, 52, 42, 119, DateTimeKind.Local).AddTicks(1246));
        }
    }
}
