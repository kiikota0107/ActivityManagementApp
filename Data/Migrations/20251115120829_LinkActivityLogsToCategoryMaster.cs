using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityManagementApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkActivityLogsToCategoryMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) まず NULL 許容の列として追加
            migrationBuilder.AddColumn<int>(
                name: "CategoryMasterId",
                table: "ActivityLogs",
                type: "INTEGER",
                nullable: true);

            // 2) 既存の Category 名 + UserId から CategoryMasterId を埋める
            migrationBuilder.Sql(@"
                UPDATE ActivityLogs
                SET CategoryMasterId = (
                    SELECT Id
                    FROM CategoryMaster
                    WHERE CategoryMaster.CategoryName = ActivityLogs.Category
                      AND CategoryMaster.UserId     = ActivityLogs.UserId
                    LIMIT 1
                );
            ");

            // 3) 万が一紐付かなかったレコードのフォールバック（ここでは 1 = '生活' 想定）
            migrationBuilder.Sql(@"
                UPDATE ActivityLogs
                SET CategoryMasterId = 1
                WHERE CategoryMasterId IS NULL;
            ");

            // 4) NOT NULL に変更（移行が終わったので）
            migrationBuilder.AlterColumn<int>(
                name: "CategoryMasterId",
                table: "ActivityLogs",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            // 5) Index & 外部キーを張る
            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CategoryMasterId",
                table: "ActivityLogs",
                column: "CategoryMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_CategoryMaster_CategoryMasterId",
                table: "ActivityLogs",
                column: "CategoryMasterId",
                principalTable: "CategoryMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_CategoryMaster_CategoryMasterId",
                table: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_CategoryMasterId",
                table: "ActivityLogs");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryMasterId",
                table: "ActivityLogs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.DropColumn(
                name: "CategoryMasterId",
                table: "ActivityLogs");
        }
    }
}
