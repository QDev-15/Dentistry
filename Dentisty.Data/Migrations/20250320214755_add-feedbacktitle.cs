using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class addfeedbacktitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FeedbackListTitle",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DoctorListSubTitle", "DoctorListTitle", "FeedbackListTitle" },
                values: new object[] { "Nhiên Nha Khoa khao khát xây dựng đội ngũ nhân viên, y bác sĩ phục vụ tận tâm, trách nhiệm đảm bảo yếu tố an toàn và chuyên nghiệp trong từng dịch vụ", "ĐỘI NGŨ CHUYÊN GIA & BÁC SĨ Nha Khoa Nhiên", "Khách hàng nói về Nha Khoa Nhiên" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ea5453e1-f640-46ad-84d2-c9a55145c545", "AQAAAAIAAYagAAAAEPpCATP1z71gbMl52HCSC41WTFyT96ztzXe5EApZleQmvzydTTW6KXZBNSDNqQWNNg==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 21, 4, 47, 54, 869, DateTimeKind.Local).AddTicks(5416));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 21, 4, 47, 54, 869, DateTimeKind.Local).AddTicks(5478));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 21, 4, 47, 54, 869, DateTimeKind.Local).AddTicks(5482));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 21, 4, 47, 54, 869, DateTimeKind.Local).AddTicks(5486));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 21, 4, 47, 54, 869, DateTimeKind.Local).AddTicks(5490));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 21, 4, 47, 54, 869, DateTimeKind.Local).AddTicks(5493));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 21, 4, 47, 54, 869, DateTimeKind.Local).AddTicks(5497));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeedbackListTitle",
                table: "AppSettings");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DoctorListSubTitle", "DoctorListTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6ceaf034-24ca-463c-9329-a66f5bde497e", "AQAAAAIAAYagAAAAEMvp6ihuVB9DR6IYT0N7L61PQ9sXpdAvqMgFQWohCzogYXuyF3vFIL4cIhC0LZ10WA==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3854));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3877));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3882));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3885));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3888));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3892));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 20, 21, 1, 46, 34, DateTimeKind.Local).AddTicks(3896));
        }
    }
}
