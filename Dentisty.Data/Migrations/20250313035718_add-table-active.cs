using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class addtableactive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveUsers",
                columns: table => new
                {
                    IpAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastActive = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveUsers", x => x.IpAddress);
                });

            migrationBuilder.CreateTable(
                name: "VisitorLogs",
                columns: table => new
                {
                    IpAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VisitTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorLogs", x => x.IpAddress);
                });

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

            migrationBuilder.CreateIndex(
                name: "ix_activeUser_time",
                table: "ActiveUsers",
                column: "LastActive");

            migrationBuilder.CreateIndex(
                name: "ix_visitorlog_time",
                table: "VisitorLogs",
                column: "VisitTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveUsers");

            migrationBuilder.DropTable(
                name: "VisitorLogs");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "45409c6b-5cd7-4aed-ae77-a65d76092d0f", "AQAAAAIAAYagAAAAEE4XMWATEyQQ6jn7FJs50VFQfhS4n2TV0IPANOQM6GQv+q8IkYIuxxg+3SByTsnT4g==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8511));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8531));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8534));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8537));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8539));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8541));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8544));
        }
    }
}
