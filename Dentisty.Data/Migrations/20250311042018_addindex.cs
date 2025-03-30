using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dentisty.Data.Migrations
{
    /// <inheritdoc />
    public partial class addindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "Doctors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "45409c6b-5cd7-4aed-ae77-a65d76092d0f", "AQAAAAIAAYagAAAAEE4XMWATEyQQ6jn7FJs50VFQfhS4n2TV0IPANOQM6GQv+q8IkYIuxxg+3SByTsnT4g==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8511));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8531));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8534));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8537));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8539));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8541));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 3, 11, 11, 20, 18, 81, DateTimeKind.Local).AddTicks(8544));

            migrationBuilder.CreateIndex(
                name: "ix_log_id",
                table: "Loggers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "ix_image_id",
                table: "Images",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "ix_doctor_name",
                table: "Doctors",
                column: "Alias");

            migrationBuilder.CreateIndex(
                name: "ix_contact_id",
                table: "Contacts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "ix_category_name_alias",
                table: "Categories",
                columns: new[] { "Name", "Alias" });

            migrationBuilder.CreateIndex(
                name: "ix_article_title_alias",
                table: "Articles",
                columns: new[] { "Title", "Alias" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_log_id",
                table: "Loggers");

            migrationBuilder.DropIndex(
                name: "ix_image_id",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "ix_doctor_name",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "ix_contact_id",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "ix_category_name_alias",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "ix_article_title_alias",
                table: "Articles");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "834bd250-8252-491b-8b33-88b90ac12360", "AQAAAAIAAYagAAAAEF7/9e7hq4pZAitwawQLft/hg0NMTt3GZ7jY5ba9PGzFfgq+FA0FoJAtEjMP9CIiAg==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(4971));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(4990));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(4993));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(4995));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(4997));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(4999));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 2, 20, 13, 52, 55, 941, DateTimeKind.Local).AddTicks(5001));
        }
    }
}
