using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityManagementApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFK_CategoryMaster_SetNull : Migration
    {
		protected override void Up(MigrationBuilder migrationBuilder)
		{
		    // 外部キーを一旦削除
		    migrationBuilder.DropForeignKey(
		        name: "FK_ActivityLogs_CategoryMaster_CategoryMasterId",
		        table: "ActivityLogs");

		    // CategoryMasterId を NULL 許容に変更
		    migrationBuilder.AlterColumn<int>(
		        name: "CategoryMasterId",
		        table: "ActivityLogs",
		        type: "INTEGER",
		        nullable: true,
		        oldClrType: typeof(int),
		        oldType: "INTEGER");

		    // ON DELETE SET NULL で外部キーを再作成
		    migrationBuilder.AddForeignKey(
		        name: "FK_ActivityLogs_CategoryMaster_CategoryMasterId",
		        table: "ActivityLogs",
		        column: "CategoryMasterId",
		        principalTable: "CategoryMaster",
		        principalColumn: "Id",
		        onDelete: ReferentialAction.SetNull
		    );
		}

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
		        oldClrType: typeof(int),
		        oldType: "INTEGER",
		        oldNullable: true);

		    migrationBuilder.AddForeignKey(
		        name: "FK_ActivityLogs_CategoryMaster_CategoryMasterId",
		        table: "ActivityLogs",
		        column: "CategoryMasterId",
		        principalTable: "CategoryMaster",
		        principalColumn: "Id",
		        onDelete: ReferentialAction.Restrict
		    );
		}
    }
}
