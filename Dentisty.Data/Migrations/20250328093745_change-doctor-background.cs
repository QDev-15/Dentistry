using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class changedoctorbackground : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Images_ImageId",
                table: "Doctors");

            migrationBuilder.AddColumn<int>(
                name: "BackgroundId",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4b7b9d71-9529-47b3-82a5-eab35e07b227", "AQAAAAIAAYagAAAAENPqboIEpK6KPvD6mHpdytGtFL3xK8Z4IbE5EtC9Ctil406iaM430zH4Zl6qVsNCFw==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4237));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4256));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4259));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4262));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4264));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4267));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 28, 16, 37, 45, 307, DateTimeKind.Local).AddTicks(4269));

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_BackgroundId",
                table: "Doctors",
                column: "BackgroundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Images_BackgroundId",
                table: "Doctors",
                column: "BackgroundId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Images_ImageId",
                table: "Doctors",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Images_BackgroundId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Images_ImageId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_BackgroundId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "BackgroundId",
                table: "Doctors");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6732446e-89f6-4c22-8299-8ae54055d648", "AQAAAAIAAYagAAAAELoMPLGj0RenYEPw78+C+fBCntdt1CgA9cVm1C68MOK7c3qeLiPNTY6RuYpQoTY0wA==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 27, 22, 1, 3, 821, DateTimeKind.Local).AddTicks(5196));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 27, 22, 1, 3, 821, DateTimeKind.Local).AddTicks(5219));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 27, 22, 1, 3, 821, DateTimeKind.Local).AddTicks(5224));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 27, 22, 1, 3, 821, DateTimeKind.Local).AddTicks(5227));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 27, 22, 1, 3, 821, DateTimeKind.Local).AddTicks(5231));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 27, 22, 1, 3, 821, DateTimeKind.Local).AddTicks(5235));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 27, 22, 1, 3, 821, DateTimeKind.Local).AddTicks(5238));

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Images_ImageId",
                table: "Doctors",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
