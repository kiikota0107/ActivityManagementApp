using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityManagementApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class activityDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "activityDetail",
                table: "ActivityLogs",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activityDetail",
                table: "ActivityLogs");
        }
    }
}
