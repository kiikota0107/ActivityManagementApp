using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityManagementApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class addDevicePairingTables_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DevicePairingCode",
                table: "DevicePairingCode");

            migrationBuilder.RenameTable(
                name: "DevicePairingCode",
                newName: "DevicePairingCodes");

            migrationBuilder.RenameIndex(
                name: "IX_DevicePairingCode_Code",
                table: "DevicePairingCodes",
                newName: "IX_DevicePairingCodes_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DevicePairingCodes",
                table: "DevicePairingCodes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DeviceToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceName = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUsedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsRevoked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceToken", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DevicePairingCodes",
                table: "DevicePairingCodes");

            migrationBuilder.RenameTable(
                name: "DevicePairingCodes",
                newName: "DevicePairingCode");

            migrationBuilder.RenameIndex(
                name: "IX_DevicePairingCodes_Code",
                table: "DevicePairingCode",
                newName: "IX_DevicePairingCode_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DevicePairingCode",
                table: "DevicePairingCode",
                column: "Id");
        }
    }
}
