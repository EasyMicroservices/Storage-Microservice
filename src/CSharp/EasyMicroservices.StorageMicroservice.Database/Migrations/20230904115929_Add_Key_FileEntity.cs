using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyMicroservices.StorageMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class Add_Key_FileEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Files",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2023, 9, 4, 15, 29, 28, 924, DateTimeKind.Local).AddTicks(9215));

            migrationBuilder.CreateIndex(
                name: "IX_Files_Key",
                table: "Files",
                column: "Key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_Key",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Files");

            migrationBuilder.UpdateData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2023, 8, 14, 17, 20, 47, 85, DateTimeKind.Local).AddTicks(420));
        }
    }
}
