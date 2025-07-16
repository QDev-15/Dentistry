using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatestatusactiveuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "ActiveUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4788a499-148b-42cf-92f9-5cf428c33b5a", "AQAAAAIAAYagAAAAEC0Z7MctXXckqizSKajoSBQdrCyAAxy0SzzT432q6gp0OJW0kzOmLYbHnbj49NbDvg==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 14, 41, 35, 972, DateTimeKind.Local).AddTicks(7159));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 14, 41, 35, 972, DateTimeKind.Local).AddTicks(7183));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 14, 41, 35, 972, DateTimeKind.Local).AddTicks(7186));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 14, 41, 35, 972, DateTimeKind.Local).AddTicks(7189));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 14, 41, 35, 972, DateTimeKind.Local).AddTicks(7191));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 14, 41, 35, 972, DateTimeKind.Local).AddTicks(7194));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 14, 41, 35, 972, DateTimeKind.Local).AddTicks(7196));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "ActiveUsers");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f50beff3-ffd3-4795-9675-3e986534528c", "AQAAAAIAAYagAAAAEEgCvcRdV3Ot5TQcU0VLn+iNBvrsMC43bDVLLHG7j8qVtqsX7aw4JESd+CWIvdaB8Q==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9301));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9325));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9328));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9331));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9333));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9336));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 15, 11, 51, 33, 485, DateTimeKind.Local).AddTicks(9338));
        }
    }
}
