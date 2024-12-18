using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedoctordescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d8155bb9-7de9-4fa3-9c4c-dbe7325902c8", "AQAAAAIAAYagAAAAEKF3MrP+0NfX9aVL6dK8x2lSp3NY8Z8VIMQ8lqjBL7be23yHin3D1FpTD42j4UNPHg==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a9a3c5ca-5c75-457c-8eb2-9f056a670469", "AQAAAAIAAYagAAAAEJXBZKAiMxh7p81stcooVlyyTfvDX1VxywtHWM+xPpQoYxbEt7Mry0xqlPrPy5dSOA==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 18, 15, 37, 9, 510, DateTimeKind.Local).AddTicks(6816));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 18, 15, 37, 9, 510, DateTimeKind.Local).AddTicks(6737));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 18, 15, 37, 9, 510, DateTimeKind.Local).AddTicks(6752));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 18, 15, 37, 9, 510, DateTimeKind.Local).AddTicks(6755));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 18, 15, 37, 9, 510, DateTimeKind.Local).AddTicks(6757));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 18, 15, 37, 9, 510, DateTimeKind.Local).AddTicks(6759));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 18, 15, 37, 9, 510, DateTimeKind.Local).AddTicks(6760));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e89a3578-b8be-4e18-bf7d-a6ae512b46b5", "AQAAAAIAAYagAAAAEKFuebJ/EC52FXMDIAwx4gADYXGmCDH0BPf+x3YKfFiWQHLp6ideQ17d1iHsRx8CzQ==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bc506c13-60ec-4242-b6d9-4055e6460cce", "AQAAAAIAAYagAAAAEOmZ3YmJ9wlFNOSqHyBlpXhwkbeHlnd6qTw1U/CCflOEr5Y+MeXGos3l2fI0fjKuXQ==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 11, 50, 2, 654, DateTimeKind.Local).AddTicks(7233));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 11, 50, 2, 654, DateTimeKind.Local).AddTicks(7166));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 11, 50, 2, 654, DateTimeKind.Local).AddTicks(7180));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 11, 50, 2, 654, DateTimeKind.Local).AddTicks(7182));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 11, 50, 2, 654, DateTimeKind.Local).AddTicks(7184));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 11, 50, 2, 654, DateTimeKind.Local).AddTicks(7186));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 11, 50, 2, 654, DateTimeKind.Local).AddTicks(7188));
        }
    }
}
