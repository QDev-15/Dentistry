using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class configurationdoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Contacts");

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hotline1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hotline2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hotline3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hotline4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zalo1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zalo2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zalo3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zalo4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Facebook1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Facebook2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Facebook3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instagram1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instagram2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instagram3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Twitter1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Twitter2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Twitter3 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PositionExtent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctors_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Contacts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d41305b8-c11e-4b74-a565-b167d08b7a31", "AQAAAAIAAYagAAAAEFs6+h7l+jdvyej0mJAqNzyqAWg7EAinKQBQ/H013qv944SwRR0mTz407CdNYK8SWw==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "732130c0-2b02-4438-b040-c5b4715255ab", "AQAAAAIAAYagAAAAEBkO+J/Bardx9kSSxrLtR1RjMFJ4S+NhokM1RyQBZmi4l1Az4OPMiCJIZdFXtNky2g==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 16, 22, 53, 57, 313, DateTimeKind.Local).AddTicks(1234));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 16, 22, 53, 57, 313, DateTimeKind.Local).AddTicks(1002));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 16, 22, 53, 57, 313, DateTimeKind.Local).AddTicks(1023));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 16, 22, 53, 57, 313, DateTimeKind.Local).AddTicks(1026));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 16, 22, 53, 57, 313, DateTimeKind.Local).AddTicks(1028));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 16, 22, 53, 57, 313, DateTimeKind.Local).AddTicks(1030));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 16, 22, 53, 57, 313, DateTimeKind.Local).AddTicks(1033));
        }
    }
}
