using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "561d796b-014d-42ad-83e1-9df7c14534b9", "AQAAAAIAAYagAAAAEM1J8cfYMEYBoK/Nrph9I2oVJTRXRza43s/tLyDCBS5XDVdJyN93+hzEvPOw4hd+Tw==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 19, 2, 39, 518, DateTimeKind.Local).AddTicks(3424));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 19, 2, 39, 518, DateTimeKind.Local).AddTicks(3450));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 19, 2, 39, 518, DateTimeKind.Local).AddTicks(3455));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 19, 2, 39, 518, DateTimeKind.Local).AddTicks(3458));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 19, 2, 39, 518, DateTimeKind.Local).AddTicks(3461));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 19, 2, 39, 518, DateTimeKind.Local).AddTicks(3465));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 19, 2, 39, 518, DateTimeKind.Local).AddTicks(3469));

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AvatarId",
                table: "Articles",
                column: "AvatarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Images_AvatarId",
                table: "Articles",
                column: "AvatarId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Images_AvatarId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_AvatarId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "Articles");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "07c29aff-e505-47cb-b6dc-bcaac58b72e7", "AQAAAAIAAYagAAAAENEr0rFe+NnTZcjJQ7K1Oks9mkPGIk9IxbpO63n3ngl/J67sFUzUlWzjtkcLn8Oyow==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 18, 17, 40, 813, DateTimeKind.Local).AddTicks(6646));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 18, 17, 40, 813, DateTimeKind.Local).AddTicks(6678));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 18, 17, 40, 813, DateTimeKind.Local).AddTicks(6686));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 18, 17, 40, 813, DateTimeKind.Local).AddTicks(6693));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 18, 17, 40, 813, DateTimeKind.Local).AddTicks(6696));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 18, 17, 40, 813, DateTimeKind.Local).AddTicks(6700));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 25, 18, 17, 40, 813, DateTimeKind.Local).AddTicks(6706));
        }
    }
}
