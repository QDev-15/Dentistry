using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatecategoryposition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsParent",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "21c585d0-e73d-4b0d-aebc-51ce5d615640", "AQAAAAIAAYagAAAAEAiv/zxTylTALjOmMpjgE1oO4J1IWNzR5ctkBJP1oPUGIsupph+biqFpon+WxNG8hQ==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7b594641-081a-4398-a17b-725d7e3d4358", "AQAAAAIAAYagAAAAEIUtvdYNqLAO026UMnSp0uRP6yxcrYQES1mEoABX+YbZiOhAd/YRKdGt4RaCXpodPw==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 14, 13, 43, 12, DateTimeKind.Local).AddTicks(9266));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 14, 13, 43, 12, DateTimeKind.Local).AddTicks(9193));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 14, 13, 43, 12, DateTimeKind.Local).AddTicks(9210));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 14, 13, 43, 12, DateTimeKind.Local).AddTicks(9212));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 14, 13, 43, 12, DateTimeKind.Local).AddTicks(9214));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 14, 13, 43, 12, DateTimeKind.Local).AddTicks(9216));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 14, 13, 43, 12, DateTimeKind.Local).AddTicks(9218));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsParent",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "71cf4c68-1f33-462f-8e38-35be143bd81f", "AQAAAAIAAYagAAAAEDOFbwcq0swBKfwOCYdbNh5UvKUp1jKlkel8sOGrp1MvWGY+YSzEyrcUVojwiRZY6A==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a062d2ab-7475-4064-974a-022113930f69", "AQAAAAIAAYagAAAAEHNy1gPLzNknlaQja4VL6jsriljm1WzCGGpjVnurY+Fa0NWZwn5PwUwg56WzBkkp0g==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 2, 14, 2, 47, 384, DateTimeKind.Local).AddTicks(1328));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 2, 14, 2, 47, 384, DateTimeKind.Local).AddTicks(1266));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 2, 14, 2, 47, 384, DateTimeKind.Local).AddTicks(1280));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 2, 14, 2, 47, 384, DateTimeKind.Local).AddTicks(1282));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 2, 14, 2, 47, 384, DateTimeKind.Local).AddTicks(1284));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 2, 14, 2, 47, 384, DateTimeKind.Local).AddTicks(1286));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 2, 14, 2, 47, 384, DateTimeKind.Local).AddTicks(1287));
        }
    }
}
