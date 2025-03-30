using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class addThumbImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbPath",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6ceaf034-24ca-463c-9329-a66f5bde497e", "AQAAAAIAAYagAAAAEMvp6ihuVB9DR6IYT0N7L61PQ9sXpdAvqMgFQWohCzogYXuyF3vFIL4cIhC0LZ10WA==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3854));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3877));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3882));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3885));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3888));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3892));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3896));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbPath",
                table: "Images");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a171dbb7-234f-45ea-adaa-b7a7ce797134", "AQAAAAIAAYagAAAAEPFVDQaNf0fQP9XwayXiFRp4KF7hj/ROX4fVMPG5jVwfnKL51MdSdqrjKaADNRb5NQ==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5022));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5039));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5042));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5045));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5047));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5049));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5052));
        }
    }
}
