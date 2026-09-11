using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerEmailConfirmationSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerEmailTemplate",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SendCustomerConfirmationEmail",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CustomerEmailTemplate", "SendCustomerConfirmationEmail" },
                values: new object[] { null, false });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerEmailTemplate",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "SendCustomerConfirmationEmail",
                table: "AppSettings");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "62e044d5-42f8-45fd-902d-e11279861514", "AQAAAAIAAYagAAAAEGC3mVldt65KVm5b/9/wNzLHpYN93PpI42LZ3HbJPp5x3P0vPfpld0mhw4pTvhNbMQ==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 11, 33, 55, 719, DateTimeKind.Local).AddTicks(7876));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 11, 33, 55, 719, DateTimeKind.Local).AddTicks(7928));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 11, 33, 55, 719, DateTimeKind.Local).AddTicks(7933));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 11, 33, 55, 719, DateTimeKind.Local).AddTicks(7937));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 11, 33, 55, 719, DateTimeKind.Local).AddTicks(7941));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 11, 33, 55, 719, DateTimeKind.Local).AddTicks(7945));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 9, 11, 11, 33, 55, 719, DateTimeKind.Local).AddTicks(7949));
        }
    }
}
