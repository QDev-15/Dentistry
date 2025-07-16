using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateuserlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "VisitorLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "VisitorLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "VisitorLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorId",
                table: "VisitorLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "ActiveUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "ActiveUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "ActiveUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorId",
                table: "ActiveUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f50beff3-ffd3-4795-9675-3e986534528c", "AQAAAAIAAYagAAAAEEgCvcRdV3Ot5TQcU0VLn+iNBvrsMC43bDVLLHG7j8qVtqsX7aw4JESd+CWIvdaB8Q==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9301));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9325));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9328));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9331));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9333));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9336));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9338));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "VisitorLogs");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "VisitorLogs");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "VisitorLogs");

            migrationBuilder.DropColumn(
                name: "VisitorId",
                table: "VisitorLogs");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "ActiveUsers");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "ActiveUsers");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "ActiveUsers");

            migrationBuilder.DropColumn(
                name: "VisitorId",
                table: "ActiveUsers");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4b7b9d71-9529-47b3-82a5-eab35e07b227", "AQAAAAIAAYagAAAAENPqboIEpK6KPvD6mHpdytGtFL3xK8Z4IbE5EtC9Ctil406iaM430zH4Zl6qVsNCFw==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4237));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4256));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4259));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4262));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4264));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4267));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4269));
        }
    }
}
