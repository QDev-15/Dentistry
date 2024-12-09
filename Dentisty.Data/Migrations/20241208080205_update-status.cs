using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatestatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Articles");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Slides",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Avtive",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d50f6f81-64e8-48c5-a7db-553b8755c163", "AQAAAAIAAYagAAAAECft7iwIab956Iq/Fopse3dJcGbEVaICmr8zhAZ7o2/m6GEta2ld1aCQRffbanUEZQ==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b7bf55fe-4a3c-4e2a-b664-5787d0a6a2df", "AQAAAAIAAYagAAAAEHhk8Hb85YxJWCeLRl+sLaJ3mlbtMWUDrYclQBVxKG98hC4l+nhjbEfCwiTIGmpYzA==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Avtive", "CreatedDate" },
                values: new object[] { true, new DateTime(2024, 12, 8, 15, 2, 4, 212, DateTimeKind.Local).AddTicks(6682) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Active", "CreatedDate" },
                values: new object[] { true, new DateTime(2024, 12, 8, 15, 2, 4, 212, DateTimeKind.Local).AddTicks(6465) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Active", "CreatedDate" },
                values: new object[] { true, new DateTime(2024, 12, 8, 15, 2, 4, 212, DateTimeKind.Local).AddTicks(6491) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Active", "CreatedDate" },
                values: new object[] { true, new DateTime(2024, 12, 8, 15, 2, 4, 212, DateTimeKind.Local).AddTicks(6494) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Active", "CreatedDate" },
                values: new object[] { true, new DateTime(2024, 12, 8, 15, 2, 4, 212, DateTimeKind.Local).AddTicks(6497) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Active", "CreatedDate" },
                values: new object[] { true, new DateTime(2024, 12, 8, 15, 2, 4, 212, DateTimeKind.Local).AddTicks(6499) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Active", "CreatedDate" },
                values: new object[] { true, new DateTime(2024, 12, 8, 15, 2, 4, 212, DateTimeKind.Local).AddTicks(6501) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Avtive",
                table: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Slides",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b012aa42-9112-4045-8529-92390eee9499", "AQAAAAIAAYagAAAAECBhNDWs6JpRFkRPIn0prUSs0kSgoSXS08Ll1BKscPz2djIozjvkc18zrEPUVNxjHg==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b077f300-1044-4910-87f7-139dd188b4dd", "AQAAAAIAAYagAAAAED9Ys7wpwfO9imXEfvJ4OVx6iT9aymCyv2uylXAwwaFRlWWSAOzR9r6QAZ4YuVXJfw==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Status" },
                values: new object[] { new DateTime(2024, 12, 6, 13, 32, 56, 455, DateTimeKind.Local).AddTicks(5429), 0 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Status" },
                values: new object[] { new DateTime(2024, 12, 6, 13, 32, 56, 455, DateTimeKind.Local).AddTicks(5347), 1 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Status" },
                values: new object[] { new DateTime(2024, 12, 6, 13, 32, 56, 455, DateTimeKind.Local).AddTicks(5366), 1 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Status" },
                values: new object[] { new DateTime(2024, 12, 6, 13, 32, 56, 455, DateTimeKind.Local).AddTicks(5374), 1 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "Status" },
                values: new object[] { new DateTime(2024, 12, 6, 13, 32, 56, 455, DateTimeKind.Local).AddTicks(5375), 1 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "Status" },
                values: new object[] { new DateTime(2024, 12, 6, 13, 32, 56, 455, DateTimeKind.Local).AddTicks(5377), 1 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "Status" },
                values: new object[] { new DateTime(2024, 12, 6, 13, 32, 56, 455, DateTimeKind.Local).AddTicks(5378), 1 });
        }
    }
}
