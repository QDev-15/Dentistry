using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateappsetting20250319 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BranchesTitle",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryListProductSubTitle",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryListProductTitle",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryListSubTitle",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryListTitle",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryProducts",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyAddress",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmail",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyPhone",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyTitle",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyWebsite",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorListSubTitle",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorListTitle",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewsListTitle",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowCategoryProductList",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Tiktok",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BranchesTitle", "CategoryListProductSubTitle", "CategoryListProductTitle", "CategoryListSubTitle", "CategoryListTitle", "CategoryProducts", "CompanyAddress", "CompanyEmail", "CompanyName", "CompanyPhone", "CompanyTitle", "CompanyWebsite", "DoctorListSubTitle", "DoctorListTitle", "NewsListTitle", "ShowCategoryProductList", "Tiktok" },
                values: new object[] { null, "Nhiên tập trung cung cấp các dịch vụ nha khoa thẩm mỹ trong môi trường nhẹ nhàng, chu đáo, chuyên nghiệp. Đảm bảo mọi khách hàng tới Win Smile đều có những trải nhiệm nha khoa đẳng cấp, ưng ý nhất cho đường cười của chính mình.", "Sản Phẩm Thẩm Mỹ", "Nhiên tập trung cung cấp các dịch vụ nha khoa thẩm mỹ trong môi trường nhẹ nhàng, chu đáo, chuyên nghiệp. Đảm bảo mọi khách hàng tới Win Smile đều có những trải nhiệm nha khoa đẳng cấp, ưng ý nhất cho đường cười của chính mình.", "Dịch Vụ Thẩm Mỹ", null, null, null, null, null, null, null, null, null, null, true, null });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a171dbb7-234f-45ea-adaa-b7a7ce797134", "AQAAAAIAAYagAAAAEPFVDQaNf0fQP9XwayXiFRp4KF7hj/ROX4fVMPG5jVwfnKL51MdSdqrjKaADNRb5NQ==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5022), true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5039), true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5042), true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5045), true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5047), true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5049), true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 19, 11, 44, 53, 655, DateTimeKind.Local).AddTicks(5052), true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "BranchesTitle",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CategoryListProductSubTitle",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CategoryListProductTitle",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CategoryListSubTitle",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CategoryListTitle",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CategoryProducts",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CompanyAddress",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CompanyEmail",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CompanyPhone",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CompanyTitle",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CompanyWebsite",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "DoctorListSubTitle",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "DoctorListTitle",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "NewsListTitle",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "ShowCategoryProductList",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Tiktok",
                table: "AppSettings");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d5728b0e-b90b-4ac6-a7fe-625b63cc01ac", "AQAAAAIAAYagAAAAENqYUtw+92JaTdZ6Cha3Pi51GYx9IZJg0Id6h54bXpt3O16uGIzF95nRkowFL2R84g==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4314), false });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4333), false });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4336), false });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4338), false });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4340), false });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4342), false });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "IsParent" },
                values: new object[] { new DateTime(2025, 3, 13, 15, 29, 10, 765, DateTimeKind.Local).AddTicks(4344), false });
        }
    }
}
