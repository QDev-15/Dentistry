using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateappsettingall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Images_AvatarId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_AppUsers_ProcessById",
                table: "Contacts");

            migrationBuilder.DropTable(
                name: "Bases");

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Facebook1",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Facebook2",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Facebook3",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Hotline1",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Hotline2",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Hotline3",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Hotline4",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Instagram1",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Instagram2",
                table: "AppSettings");

            migrationBuilder.RenameColumn(
                name: "Zalo4",
                table: "AppSettings",
                newName: "ZaloHotline");

            migrationBuilder.RenameColumn(
                name: "Zalo3",
                table: "AppSettings",
                newName: "Youtube");

            migrationBuilder.RenameColumn(
                name: "Zalo2",
                table: "AppSettings",
                newName: "Twitter");

            migrationBuilder.RenameColumn(
                name: "Zalo1",
                table: "AppSettings",
                newName: "StartWork");

            migrationBuilder.RenameColumn(
                name: "Twitter3",
                table: "AppSettings",
                newName: "Instagram");

            migrationBuilder.RenameColumn(
                name: "Twitter2",
                table: "AppSettings",
                newName: "HotlineHaNoi");

            migrationBuilder.RenameColumn(
                name: "Twitter1",
                table: "AppSettings",
                newName: "Facebook");

            migrationBuilder.RenameColumn(
                name: "Instagram3",
                table: "AppSettings",
                newName: "EndWork");

            migrationBuilder.AddColumn<int>(
                name: "BranchesId",
                table: "Contacts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowArtileSlideList",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowCategoryList",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowContactList",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowDoctorlideList",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowFeedbackList",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowNewsList",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowProductList",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowToolBarTop",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

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

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-945a-435d-bba4-df3f325983dc"),
                column: "NormalizedName",
                value: "NICK");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "NormalizedName",
                value: "ADMIN");

            migrationBuilder.InsertData(
                table: "AppSettings",
                columns: new[] { "Id", "EndWork", "Facebook", "HotlineHaNoi", "Instagram", "Name", "ShowArtileSlideList", "ShowCategoryList", "ShowContactList", "ShowDoctorlideList", "ShowFeedbackList", "ShowNewsList", "ShowProductList", "ShowToolBarTop", "StartWork", "Twitter", "Youtube", "ZaloHotline" },
                values: new object[] { 1, null, null, null, null, "Nhiên Dentistry", true, true, true, true, true, true, true, true, null, null, null, null });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), new Guid("69bd714f-9576-45ca-b5b7-f00649be00de") });

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

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_BranchesId",
                table: "Contacts",
                column: "BranchesId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTags_TagId",
                table: "ArticleTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Images_AvatarId",
                table: "AppUsers",
                column: "AvatarId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_AppUsers_ProcessById",
                table: "Contacts",
                column: "ProcessById",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Branches_BranchesId",
                table: "Contacts",
                column: "BranchesId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Images_AvatarId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_AppUsers_ProcessById",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Branches_BranchesId",
                table: "Contacts");

            migrationBuilder.DropTable(
                name: "ArticleTags");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_BranchesId",
                table: "Contacts");

            migrationBuilder.DeleteData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), new Guid("69bd714f-9576-45ca-b5b7-f00649be00de") });

            migrationBuilder.DropColumn(
                name: "BranchesId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "ShowArtileSlideList",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "ShowCategoryList",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "ShowContactList",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "ShowDoctorlideList",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "ShowFeedbackList",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "ShowNewsList",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "ShowProductList",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "ShowToolBarTop",
                table: "AppSettings");

            migrationBuilder.RenameColumn(
                name: "ZaloHotline",
                table: "AppSettings",
                newName: "Zalo4");

            migrationBuilder.RenameColumn(
                name: "Youtube",
                table: "AppSettings",
                newName: "Zalo3");

            migrationBuilder.RenameColumn(
                name: "Twitter",
                table: "AppSettings",
                newName: "Zalo2");

            migrationBuilder.RenameColumn(
                name: "StartWork",
                table: "AppSettings",
                newName: "Zalo1");

            migrationBuilder.RenameColumn(
                name: "Instagram",
                table: "AppSettings",
                newName: "Twitter3");

            migrationBuilder.RenameColumn(
                name: "HotlineHaNoi",
                table: "AppSettings",
                newName: "Twitter2");

            migrationBuilder.RenameColumn(
                name: "Facebook",
                table: "AppSettings",
                newName: "Twitter1");

            migrationBuilder.RenameColumn(
                name: "EndWork",
                table: "AppSettings",
                newName: "Instagram3");

            migrationBuilder.AddColumn<string>(
                name: "Facebook1",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facebook2",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facebook3",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hotline1",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hotline2",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hotline3",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hotline4",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram1",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram2",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Map = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bases", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppConfigs",
                columns: new[] { "Id", "Key", "Value" },
                values: new object[,]
                {
                    { 1, "hotline-hcm", "19009090" },
                    { 2, "facebook", "nhien86" },
                    { 3, "hotline-hanoi", "19001010" }
                });

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-945a-435d-bba4-df3f325983dc"),
                column: "NormalizedName",
                value: "User");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "NormalizedName",
                value: "admin");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "060ff20c-32e0-4bd6-a4c3-a4b0e334e8a9", "AQAAAAIAAYagAAAAEEyCy9aMPSk/dYsb8QlH86BEOUCFJCUugdBjiWWykpbBqm73ZU6uvH/qbXe0Q1zzfQ==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ca-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "04e0b0e8-d98f-4198-b946-9c5f11660d3d", "AQAAAAIAAYagAAAAEK9GVwlki+DpQhqaNTukIL0KsJhTrlfwHNNScYMlZC/LKdG3Cekfs1FK6JJPYUN2KA==" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 19, 13, 3, 22, 648, DateTimeKind.Local).AddTicks(1200));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 19, 13, 3, 22, 646, DateTimeKind.Local).AddTicks(3447));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 19, 13, 3, 22, 646, DateTimeKind.Local).AddTicks(3467));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 19, 13, 3, 22, 646, DateTimeKind.Local).AddTicks(3469));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 19, 13, 3, 22, 646, DateTimeKind.Local).AddTicks(3471));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 19, 13, 3, 22, 646, DateTimeKind.Local).AddTicks(3473));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 19, 13, 3, 22, 646, DateTimeKind.Local).AddTicks(3475));

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Code", "IsDefault", "Name" },
                values: new object[,]
                {
                    { 1, "vi", true, "Tiếng Việt" },
                    { 2, "en", false, "English" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Images_AvatarId",
                table: "AppUsers",
                column: "AvatarId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_AppUsers_ProcessById",
                table: "Contacts",
                column: "ProcessById",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
