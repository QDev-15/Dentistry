using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitorTrackingLookupIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VisitorId",
                table: "VisitorLogs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "VisitorLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VisitorId",
                table: "ActiveUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "ActiveUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "71c6440e-fbdf-4c64-894d-71e78d1b28d0", "AQAAAAIAAYagAAAAEM3C0lw6P/hEpIBEnjnwF5Lw/eBO7tt7FC2NBRKhgR7DSiWlpwrK8JK1zXgUv60XVw==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 15, 12, 10, 231, DateTimeKind.Local).AddTicks(2823));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 15, 12, 10, 231, DateTimeKind.Local).AddTicks(2855));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 15, 12, 10, 231, DateTimeKind.Local).AddTicks(2860));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 15, 12, 10, 231, DateTimeKind.Local).AddTicks(2864));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 15, 12, 10, 231, DateTimeKind.Local).AddTicks(2868));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 15, 12, 10, 231, DateTimeKind.Local).AddTicks(2872));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 15, 12, 10, 231, DateTimeKind.Local).AddTicks(2875));

            migrationBuilder.CreateIndex(
                name: "ix_visitorlog_visitor_ip",
                table: "VisitorLogs",
                columns: new[] { "VisitorId", "IpAddress" });

            migrationBuilder.CreateIndex(
                name: "ix_activeUser_visitor_ip",
                table: "ActiveUsers",
                columns: new[] { "VisitorId", "IpAddress" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_visitorlog_visitor_ip",
                table: "VisitorLogs");

            migrationBuilder.DropIndex(
                name: "ix_activeUser_visitor_ip",
                table: "ActiveUsers");

            migrationBuilder.AlterColumn<string>(
                name: "VisitorId",
                table: "VisitorLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "VisitorLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "VisitorId",
                table: "ActiveUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "ActiveUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "95d1b767-3239-4523-ad0e-f9dd11eab9c7", "AQAAAAIAAYagAAAAEPNtPMGdQvdmhl+v9v2lmbYG7//5i1mWuhI5ptdQq1DZ9NSeyX9OEmf2F7ppLkvF3Q==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 12, 35, 1, 738, DateTimeKind.Local).AddTicks(3186));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 12, 35, 1, 738, DateTimeKind.Local).AddTicks(3220));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 12, 35, 1, 738, DateTimeKind.Local).AddTicks(3226));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 12, 35, 1, 738, DateTimeKind.Local).AddTicks(3231));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 12, 35, 1, 738, DateTimeKind.Local).AddTicks(3238));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 12, 35, 1, 738, DateTimeKind.Local).AddTicks(3243));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 12, 35, 1, 738, DateTimeKind.Local).AddTicks(3248));
        }
    }
}
