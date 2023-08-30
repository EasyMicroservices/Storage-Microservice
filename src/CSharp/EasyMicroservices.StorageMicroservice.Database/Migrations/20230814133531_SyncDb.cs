using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyMicroservices.StorageMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class SyncDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2023, 8, 14, 17, 5, 31, 597, DateTimeKind.Local).AddTicks(9609));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2023, 8, 10, 17, 50, 36, 738, DateTimeKind.Local).AddTicks(6739));
        }
    }
}
