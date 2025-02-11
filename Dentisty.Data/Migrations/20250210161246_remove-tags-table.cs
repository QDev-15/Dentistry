using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class removetagstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Tags_TagsId",
                table: "Articles");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Articles_TagsId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "TagsId",
                table: "Articles");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0e55acbc-c5bf-4c3d-8dbf-a102f3b1bc24", "AQAAAAIAAYagAAAAENFX7B6VP58ccCtkNN/OwwgvrohyD10viXOYE6Q79nAdrTybnyM7eyFA9f4cidGFmw==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4669fbb6-7229-4b14-9837-7553f27c32c7", "AQAAAAIAAYagAAAAEEWd8GSwxDxh9pzZtAMex8sOmkPpBPHlVwVBdOId8GZBF6TyYxJUeb5DK1UMXbAsJA==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 10, 23, 12, 45, 309, DateTimeKind.Local).AddTicks(8504));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 10, 23, 12, 45, 309, DateTimeKind.Local).AddTicks(8376));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 10, 23, 12, 45, 309, DateTimeKind.Local).AddTicks(8397));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 10, 23, 12, 45, 309, DateTimeKind.Local).AddTicks(8400));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 10, 23, 12, 45, 309, DateTimeKind.Local).AddTicks(8403));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 10, 23, 12, 45, 309, DateTimeKind.Local).AddTicks(8405));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 10, 23, 12, 45, 309, DateTimeKind.Local).AddTicks(8407));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagsId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8cab0b64-cf0d-41c3-9f82-51d98adbb8a5", "AQAAAAIAAYagAAAAEKCze0nGVZGjvDJwssX/GhmzHGI41bvx4RAOGbFwjWQ5BoDL8EN8BPL8H39eYYpQOg==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5b4e80d0-ca40-4ec8-b066-be2cba7577c6", "AQAAAAIAAYagAAAAEAF1HlwgvFYZd5x+PnyEBP65rPeqiqRchGWE1EL8DzgqVGLP2KCn+tFFbJXSTBCNsw==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "TagsId" },
                values: new object[] { new DateTime(2025, 2, 10, 23, 2, 47, 479, DateTimeKind.Local).AddTicks(2970), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 10, 23, 2, 47, 479, DateTimeKind.Local).AddTicks(2845));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 10, 23, 2, 47, 479, DateTimeKind.Local).AddTicks(2867));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 10, 23, 2, 47, 479, DateTimeKind.Local).AddTicks(2869));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 10, 23, 2, 47, 479, DateTimeKind.Local).AddTicks(2872));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 10, 23, 2, 47, 479, DateTimeKind.Local).AddTicks(2874));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 10, 23, 2, 47, 479, DateTimeKind.Local).AddTicks(2877));

            migrationBuilder.CreateIndex(
                name: "IX_Articles_TagsId",
                table: "Articles",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Tags_TagsId",
                table: "Articles",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
