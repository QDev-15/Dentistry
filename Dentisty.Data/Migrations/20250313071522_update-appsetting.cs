using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateappsetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TrackVisitors",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "TrackVisitors",
                value: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3703cbc0-e25d-4c19-beb9-4b477391915e", "AQAAAAIAAYagAAAAEFqLF78L81Elbutns/3CAOk14cxWXOf6LfRtRbExZyZ/eFp+wSI2cxicF2qpqVsRAw==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 14, 15, 22, 511, DateTimeKind.Local).AddTicks(8732));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 14, 15, 22, 511, DateTimeKind.Local).AddTicks(8749));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 14, 15, 22, 511, DateTimeKind.Local).AddTicks(8752));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 14, 15, 22, 511, DateTimeKind.Local).AddTicks(8754));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 14, 15, 22, 511, DateTimeKind.Local).AddTicks(8777));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 14, 15, 22, 511, DateTimeKind.Local).AddTicks(8779));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 14, 15, 22, 511, DateTimeKind.Local).AddTicks(8781));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackVisitors",
                table: "AppSettings");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c95fdd19-3d04-4620-ab02-516d708810c1", "AQAAAAIAAYagAAAAEChpb+YobAJKYbLxDmshAL2apgJEfbiWvytYnNgqB5wewtLBbot2U1KrqL8/jxFH2g==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 10, 57, 17, 656, DateTimeKind.Local).AddTicks(800));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 10, 57, 17, 656, DateTimeKind.Local).AddTicks(816));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 10, 57, 17, 656, DateTimeKind.Local).AddTicks(819));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 10, 57, 17, 656, DateTimeKind.Local).AddTicks(822));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 10, 57, 17, 656, DateTimeKind.Local).AddTicks(825));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 10, 57, 17, 656, DateTimeKind.Local).AddTicks(827));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 10, 57, 17, 656, DateTimeKind.Local).AddTicks(830));
        }
    }
}
