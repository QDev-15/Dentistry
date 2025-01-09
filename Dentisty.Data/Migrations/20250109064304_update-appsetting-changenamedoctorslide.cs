using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateappsettingchangenamedoctorslide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShowDoctorlideList",
                table: "AppSettings",
                newName: "ShowDoctorSlideList");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a5ce22b8-5650-4253-991b-09886a1b856c", "AQAAAAIAAYagAAAAEPkhE1r3vtGh2iXiE2rIRB7Q2m0YK1AarLdFKfvP7sS4q6mIyHMIh211grKmu4fM6g==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e259eecf-5b34-4438-ab0f-3ffbc7b5fe7d", "AQAAAAIAAYagAAAAEMJkKcWQFwyvKlO0HhRRk1iSRYKVJPZfQ8UhpHFwOO18XUoVK7uEd6yCvrwFOtSPBA==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Type" },
                values: new object[] { new DateTime(2025, 1, 9, 13, 43, 4, 210, DateTimeKind.Local).AddTicks(4250), 0 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 9, 13, 43, 4, 210, DateTimeKind.Local).AddTicks(4183));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 9, 13, 43, 4, 210, DateTimeKind.Local).AddTicks(4197));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 9, 13, 43, 4, 210, DateTimeKind.Local).AddTicks(4200));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 9, 13, 43, 4, 210, DateTimeKind.Local).AddTicks(4201));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 9, 13, 43, 4, 210, DateTimeKind.Local).AddTicks(4204));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 9, 13, 43, 4, 210, DateTimeKind.Local).AddTicks(4206));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "ShowDoctorSlideList",
                table: "AppSettings",
                newName: "ShowDoctorlideList");

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
    }
}
