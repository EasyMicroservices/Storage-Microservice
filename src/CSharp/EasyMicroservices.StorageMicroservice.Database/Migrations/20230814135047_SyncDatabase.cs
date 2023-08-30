using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyMicroservices.StorageMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class SyncDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2023, 8, 14, 17, 20, 47, 85, DateTimeKind.Local).AddTicks(420));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2023, 8, 14, 17, 5, 31, 597, DateTimeKind.Local).AddTicks(9609));
        }
    }
}
