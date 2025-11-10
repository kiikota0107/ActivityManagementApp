using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityManagementApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class renameActivityDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "activityDetail",
                table: "ActivityLogs",
                newName: "ActivityDetail");

            migrationBuilder.AddColumn<string>(
                name: "ActivityDetailTitle",
                table: "ActivityLogs",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityDetailTitle",
                table: "ActivityLogs");

            migrationBuilder.RenameColumn(
                name: "ActivityDetail",
                table: "ActivityLogs",
                newName: "activityDetail");
        }
    }
}
