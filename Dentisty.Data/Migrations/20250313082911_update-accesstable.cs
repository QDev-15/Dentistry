using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateaccesstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VisitorLogs",
                table: "VisitorLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveUsers",
                table: "ActiveUsers");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "VisitorLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VisitorLogs",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "ActiveUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ActiveUsers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VisitorLogs",
                table: "VisitorLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveUsers",
                table: "ActiveUsers",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d5728b0e-b90b-4ac6-a7fe-625b63cc01ac", "AQAAAAIAAYagAAAAENqYUtw+92JaTdZ6Cha3Pi51GYx9IZJg0Id6h54bXpt3O16uGIzF95nRkowFL2R84g==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4314));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4333));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4336));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4338));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4340));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4342));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4344));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VisitorLogs",
                table: "VisitorLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveUsers",
                table: "ActiveUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VisitorLogs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ActiveUsers");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "VisitorLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "ActiveUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VisitorLogs",
                table: "VisitorLogs",
                column: "IpAddress");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveUsers",
                table: "ActiveUsers",
                column: "IpAddress");

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
    }
}
