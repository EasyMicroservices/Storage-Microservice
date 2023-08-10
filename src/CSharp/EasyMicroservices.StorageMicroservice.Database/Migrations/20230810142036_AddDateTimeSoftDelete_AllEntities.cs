using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyMicroservices.StorageMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class AddDateTimeSoftDelete_AllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UniqueIdentity",
                table: "Folders",
                type: "nvarchar(450)",
                nullable: true,
                collation: "SQL_Latin1_General_CP1_CS_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "Folders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Folders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "UniqueIdentity",
                table: "Files",
                type: "nvarchar(450)",
                nullable: true,
                collation: "SQL_Latin1_General_CP1_CS_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "Files",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Files",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreationDateTime", "DeletedDateTime", "IsDeleted" },
                values: new object[] { new DateTime(2023, 8, 10, 17, 50, 36, 738, DateTimeKind.Local).AddTicks(6739), null, false });

            migrationBuilder.CreateIndex(
                name: "IX_Folders_CreationDateTime",
                table: "Folders",
                column: "CreationDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_DeletedDateTime",
                table: "Folders",
                column: "DeletedDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_IsDeleted",
                table: "Folders",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ModificationDateTime",
                table: "Folders",
                column: "ModificationDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_UniqueIdentity",
                table: "Folders",
                column: "UniqueIdentity");

            migrationBuilder.CreateIndex(
                name: "IX_Files_CreationDateTime",
                table: "Files",
                column: "CreationDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Files_DeletedDateTime",
                table: "Files",
                column: "DeletedDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Files_IsDeleted",
                table: "Files",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Files_ModificationDateTime",
                table: "Files",
                column: "ModificationDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Files_UniqueIdentity",
                table: "Files",
                column: "UniqueIdentity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Folders_CreationDateTime",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_DeletedDateTime",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_IsDeleted",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_ModificationDateTime",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_UniqueIdentity",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Files_CreationDateTime",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_DeletedDateTime",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_IsDeleted",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_ModificationDateTime",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_UniqueIdentity",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Files");

            migrationBuilder.AlterColumn<string>(
                name: "UniqueIdentity",
                table: "Folders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true,
                oldCollation: "SQL_Latin1_General_CP1_CS_AS");

            migrationBuilder.AlterColumn<string>(
                name: "UniqueIdentity",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true,
                oldCollation: "SQL_Latin1_General_CP1_CS_AS");

            migrationBuilder.UpdateData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2023, 8, 10, 17, 30, 0, 427, DateTimeKind.Local).AddTicks(7466));
        }
    }
}
