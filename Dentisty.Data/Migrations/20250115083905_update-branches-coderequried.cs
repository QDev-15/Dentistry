using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatebranchescoderequried : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Branches",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Branches",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "357964ea-d075-4062-9081-a8129251aa29", "AQAAAAIAAYagAAAAEDd/6k2EKJtI6OJILdlTaD9xzPyYYiLNCV+CG2JJyl8EjKfkBe10ZZKsubpjd3D+rQ==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4347d86c-53f7-4e04-a16c-3f1a2a668986", "AQAAAAIAAYagAAAAEMyzjcC9f1bRNjF647Bfhk+Mbz1+59oWKWkwKy2c3wTdk3i0uLwWmxC6O8qGr/6F5g==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 13, 11, 5, 12, 556, DateTimeKind.Local).AddTicks(9883));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 13, 11, 5, 12, 556, DateTimeKind.Local).AddTicks(9812));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 13, 11, 5, 12, 556, DateTimeKind.Local).AddTicks(9830));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 13, 11, 5, 12, 556, DateTimeKind.Local).AddTicks(9832));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 13, 11, 5, 12, 556, DateTimeKind.Local).AddTicks(9833));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 13, 11, 5, 12, 556, DateTimeKind.Local).AddTicks(9835));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 13, 11, 5, 12, 556, DateTimeKind.Local).AddTicks(9837));
        }
    }
}
