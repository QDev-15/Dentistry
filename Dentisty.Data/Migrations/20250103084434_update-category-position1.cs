using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatecategoryposition1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "65d2d93a-8670-40f7-bd29-9ee1c8737cce", "AQAAAAIAAYagAAAAEFcw0Vmnk/mwhMk9OdyYHfFO4c3IElQQPtxfF+jb6IF0kg88lu6f5WufVMgAlp9nMA==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f0f65094-f8ea-4b44-8ca3-a3f8f2072430", "AQAAAAIAAYagAAAAEK6LqnxinxpjkkWqcrOeU45yofrIP6C3wQGTRWz7IUrkTeDijLdxMRk2PVW616wYRA==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 15, 44, 33, 806, DateTimeKind.Local).AddTicks(7324));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 15, 44, 33, 806, DateTimeKind.Local).AddTicks(7261));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 15, 44, 33, 806, DateTimeKind.Local).AddTicks(7280));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 15, 44, 33, 806, DateTimeKind.Local).AddTicks(7282));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 15, 44, 33, 806, DateTimeKind.Local).AddTicks(7284));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 15, 44, 33, 806, DateTimeKind.Local).AddTicks(7285));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 1, 3, 15, 44, 33, 806, DateTimeKind.Local).AddTicks(7287));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
