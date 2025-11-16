using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityManagementApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeCategoryMasterNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_CategoryMaster_CategoryMasterId",
                table: "ActivityLogs");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryMasterId",
                table: "ActivityLogs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_CategoryMaster_CategoryMasterId",
                table: "ActivityLogs",
                column: "CategoryMasterId",
                principalTable: "CategoryMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_CategoryMaster_CategoryMasterId",
                table: "ActivityLogs");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryMasterId",
                table: "ActivityLogs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_CategoryMaster_CategoryMasterId",
                table: "ActivityLogs",
                column: "CategoryMasterId",
                principalTable: "CategoryMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
