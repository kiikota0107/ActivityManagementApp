using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityManagementApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class addSkipAppLocFlgToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SkipAppLockOnActiveFlg",
                table: "CategoryMaster",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkipAppLockOnActiveFlg",
                table: "CategoryMaster");
        }
    }
}
