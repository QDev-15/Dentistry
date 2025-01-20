using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatecategorytype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "fe962730-c8d2-4217-915b-52c694423f3a", "AQAAAAIAAYagAAAAELsKA4m3Nw04NTLvJsS78FVL4ujgzu57qk5KBqoddnCJdOGbhCgLJt7StO5n5gNYfw==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8ca50b97-7a75-4ae5-befa-7bb317e5783f", "AQAAAAIAAYagAAAAEFDtxWHr0hdULTb2Vp6fdfXMjYx1WePijs8Qg1pnICeGW8PU6lu9ojVWdkgLL5HKuw==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 15, 15, 39, 5, 406, DateTimeKind.Local).AddTicks(7729));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 15, 15, 39, 5, 406, DateTimeKind.Local).AddTicks(7666));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 15, 15, 39, 5, 406, DateTimeKind.Local).AddTicks(7681));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 15, 15, 39, 5, 406, DateTimeKind.Local).AddTicks(7683));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 15, 15, 39, 5, 406, DateTimeKind.Local).AddTicks(7685));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 15, 15, 39, 5, 406, DateTimeKind.Local).AddTicks(7688));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 15, 15, 39, 5, 406, DateTimeKind.Local).AddTicks(7690));
        }
    }
}
