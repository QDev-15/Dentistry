using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class changetagarticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "ArticleTags");

            migrationBuilder.DropTable(
                name: "HomeArticles");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TagsId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-945a-435d-bba4-df3f325983dc"),
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "user", "USER" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash" },
                values: new object[] { "8cab0b64-cf0d-41c3-9f82-51d98adbb8a5", "ADMIN", "AQAAAAIAAYagAAAAEKCze0nGVZGjvDJwssX/GhmzHGI41bvx4RAOGbFwjWQ5BoDL8EN8BPL8H39eYYpQOg==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UserName" },
                values: new object[] { "5b4e80d0-ca40-4ec8-b066-be2cba7577c6", "AQAAAAIAAYagAAAAEAF1HlwgvFYZd5x+PnyEBP65rPeqiqRchGWE1EL8DzgqVGLP2KCn+tFFbJXSTBCNsw==", "userdefault" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Tags", "TagsId" },
                values: new object[] { new DateTime(2025, 2, 10, 23, 2, 47, 479, DateTimeKind.Local).AddTicks(2970), null, null });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Tags_TagsId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_TagsId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "TagsId",
                table: "Articles");

            migrationBuilder.CreateTable(
                name: "ArticleTags",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTags", x => new { x.ArticleId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ArticleTags_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HomeArticles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    BackgroundImageId = table.Column<int>(type: "int", nullable: false),
                    ThemeStyle = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeArticles_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeArticles_Images_BackgroundImageId",
                        column: x => x.BackgroundImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-945a-435d-bba4-df3f325983dc"),
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Nick", "NICK" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash" },
                values: new object[] { "e88e21ff-f919-4f62-a466-09cf3bff16e9", "admin", "AQAAAAIAAYagAAAAEOfG26f1W3Ae5iJHdXstsVto0/aH3UdSZm4SLRVtRygMOGBdXHISk1WLtDnz1BzOWg==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UserName" },
                values: new object[] { "8451aa28-0afb-453d-ba7f-ccc317c4248c", "AQAAAAIAAYagAAAAEFhcHLxbuGL9qjt5K0HK5msey83/eigaP6LYBPBjA9TQpy/VDf9EzKS9kjRCvRlknQ==", "User Default" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3736));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3619));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3670));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3672));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3673));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3675));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 3, 15, 17, 42, 437, DateTimeKind.Local).AddTicks(3677));

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTags_TagId",
                table: "ArticleTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeArticles_ArticleId",
                table: "HomeArticles",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeArticles_BackgroundImageId",
                table: "HomeArticles",
                column: "BackgroundImageId");
        }
    }
}
