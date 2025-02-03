using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class changecontactdateform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeBook",
                table: "Contacts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e88e21ff-f919-4f62-a466-09cf3bff16e9", "AQAAAAIAAYagAAAAEOfG26f1W3Ae5iJHdXstsVto0/aH3UdSZm4SLRVtRygMOGBdXHISk1WLtDnz1BzOWg==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8451aa28-0afb-453d-ba7f-ccc317c4248c", "AQAAAAIAAYagAAAAEFhcHLxbuGL9qjt5K0HK5msey83/eigaP6LYBPBjA9TQpy/VDf9EzKS9kjRCvRlknQ==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3736));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3619));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3670));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3672));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3673));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3675));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3677));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeBook",
                table: "Contacts");

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
    }
}
