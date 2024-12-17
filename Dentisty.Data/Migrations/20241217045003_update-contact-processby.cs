using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatecontactprocessby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Doctors_ImageId",
                table: "Doctors");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcessById",
                table: "Contacts",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ImageId",
                table: "Doctors",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ProcessById",
                table: "Contacts",
                column: "ProcessById");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_AppUsers_ProcessById",
                table: "Contacts",
                column: "ProcessById",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_AppUsers_ProcessById",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_ImageId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_ProcessById",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "ProcessById",
                table: "Contacts");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "640cf8fa-08f3-4002-8d3a-ed99e7c89603", "AQAAAAIAAYagAAAAEJGZ97inMzKYKijQ5Fo6HoMFsCCVL1CjOjrVCBls35qJroYxH3Jj1MDgMR7AIpH/2w==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "69f56f62-c26d-4a15-81d8-0bebe0a9998c", "AQAAAAIAAYagAAAAENFtPa58tlIgdny98DRHAsKDpjXvERZi7oXHMbjlRSqsBwe5oBXlz2vwXb9PMEyt7w==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 10, 47, 30, 699, DateTimeKind.Local).AddTicks(1054));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 10, 47, 30, 699, DateTimeKind.Local).AddTicks(995));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 10, 47, 30, 699, DateTimeKind.Local).AddTicks(1009));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 10, 47, 30, 699, DateTimeKind.Local).AddTicks(1010));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 10, 47, 30, 699, DateTimeKind.Local).AddTicks(1012));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 10, 47, 30, 699, DateTimeKind.Local).AddTicks(1013));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 17, 10, 47, 30, 699, DateTimeKind.Local).AddTicks(1015));

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ImageId",
                table: "Doctors",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");
        }
    }
}
