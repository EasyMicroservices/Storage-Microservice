using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyMicroservices.StorageMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class AddExample : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "UniqueIdentity",
                table: "Folders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniqueIdentity",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreationDateTime", "UniqueIdentity" },
                values: new object[] { new DateTime(2023, 8, 10, 17, 30, 0, 427, DateTimeKind.Local).AddTicks(7466), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueIdentity",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "UniqueIdentity",
                table: "Files");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Files",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2023, 7, 18, 8, 12, 42, 979, DateTimeKind.Local).AddTicks(7031));
        }
    }
}
