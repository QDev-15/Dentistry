using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateappsettingaeditcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Articles",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Doctors",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feedbacks",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "News",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Products",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Articles", "Categories", "Doctors", "Feedbacks", "News", "Products" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a6382ca3-7ba2-49fa-b044-e726ee24485a", "AQAAAAIAAYagAAAAEFi9AktEW2UfarasoY3PIELqJYm67oNWuCrn+qW+09PfgYNjgmJE2BcbKIucVAQD0w==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "93ad2ae0-21a7-4b38-abd2-409001f7f083", "AQAAAAIAAYagAAAAEOZZ7HoC/uVm0L9vy8Vkyh1BMBGr2QxuYdUJSn55/ChOx5SkQ/luLfozHc+ZAL6nMA==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 8, 8, 49, 4, 30, DateTimeKind.Local).AddTicks(7977));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 8, 8, 49, 4, 30, DateTimeKind.Local).AddTicks(7883));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 8, 8, 49, 4, 30, DateTimeKind.Local).AddTicks(7911));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 8, 8, 49, 4, 30, DateTimeKind.Local).AddTicks(7914));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 8, 8, 49, 4, 30, DateTimeKind.Local).AddTicks(7915));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 8, 8, 49, 4, 30, DateTimeKind.Local).AddTicks(7917));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 8, 8, 49, 4, 30, DateTimeKind.Local).AddTicks(7919));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Articles",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Categories",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Doctors",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Feedbacks",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "News",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Products",
                table: "AppSettings");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9a121dc0-55de-48a4-b75a-7cb1b62fbdd3", "AQAAAAIAAYagAAAAECtSstTN66INH74Xh890UfGabI1UMNMfH4r5Ml5zJT86XCYFFN2qgp+JGaX2dPdTcg==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1db4a9e0-c2f1-45b0-b4be-0457ee3e05d5", "AQAAAAIAAYagAAAAEJRcmQ4ZZ9yGio2t1X4SYRyjFx3XJf+vTJH7Uk97VHyQgM1GHltHp5n1SHjK3RN4Uw==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 6, 14, 24, 32, 3, DateTimeKind.Local).AddTicks(1629));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 6, 14, 24, 32, 3, DateTimeKind.Local).AddTicks(1560));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 6, 14, 24, 32, 3, DateTimeKind.Local).AddTicks(1576));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 6, 14, 24, 32, 3, DateTimeKind.Local).AddTicks(1578));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 6, 14, 24, 32, 3, DateTimeKind.Local).AddTicks(1579));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 6, 14, 24, 32, 3, DateTimeKind.Local).AddTicks(1581));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 6, 14, 24, 32, 3, DateTimeKind.Local).AddTicks(1583));
        }
    }
}
