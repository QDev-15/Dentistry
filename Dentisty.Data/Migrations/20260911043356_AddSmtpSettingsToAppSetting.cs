using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSmtpSettingsToAppSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotificationEmails",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmtpHost",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmtpPasswordEncrypted",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SmtpPort",
                table: "AppSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmtpProvider",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmtpSenderName",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SmtpUseSsl",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SmtpUsername",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "NotificationEmails", "SmtpHost", "SmtpPasswordEncrypted", "SmtpPort", "SmtpProvider", "SmtpSenderName", "SmtpUseSsl", "SmtpUsername" },
                values: new object[] { null, null, null, null, null, null, true, null });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationEmails",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "SmtpHost",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "SmtpPasswordEncrypted",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "SmtpPort",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "SmtpProvider",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "SmtpSenderName",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "SmtpUseSsl",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "SmtpUsername",
                table: "AppSettings");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ceb37cf3-d883-4eaf-85be-c4b53c3bb2d1", "AQAAAAIAAYagAAAAEF8FprKiZXQQr86kvVCVMt5H4VJSftH/oFvZ7gCVMNsAt1mWQrsQB5uLgf3MXZOENQ==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 8, 21, 11, 50, 17, 245, DateTimeKind.Local).AddTicks(7940));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 8, 21, 11, 50, 17, 245, DateTimeKind.Local).AddTicks(7965));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 8, 21, 11, 50, 17, 245, DateTimeKind.Local).AddTicks(7971));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 8, 21, 11, 50, 17, 245, DateTimeKind.Local).AddTicks(7977));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 8, 21, 11, 50, 17, 245, DateTimeKind.Local).AddTicks(7982));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 8, 21, 11, 50, 17, 245, DateTimeKind.Local).AddTicks(7987));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 8, 21, 11, 50, 17, 245, DateTimeKind.Local).AddTicks(7992));
        }
    }
}
