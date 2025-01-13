using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedoctoraddalias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Doctors");

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
                column: "CreatedDate",
                value: new DateTime(2025, 1, 9, 13, 43, 4, 210, DateTimeKind.Local).AddTicks(4250));

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
    }
}
