using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityManagementApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoryMaster_LinkToCategoryTypeMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
	{
	    // ① まず列名を変更（値はそのまま 0,1,2,3,4 が残る）
	    migrationBuilder.RenameColumn(
	        name: "CategoryType",
	        table: "CategoryMaster",
	        newName: "CategoryTypeMasterId");

	    // ② CategoryTypeMaster 側に、対応するマスタ行を作成（ID を 0〜4 に合わせる）
	    migrationBuilder.Sql(@"
	INSERT INTO CategoryTypeMaster (Id, TypeName, UserId)
	VALUES 
	    (0, '生活', ''),
	    (1, '娯楽', ''),
	    (2, '自己研鑽', ''),
	    (3, '睡眠', ''),
	    (4, '仕事', '');
	");

	    migrationBuilder.CreateIndex(
	        name: "IX_CategoryMaster_CategoryTypeMasterId",
	        table: "CategoryMaster",
	        column: "CategoryTypeMasterId");

	    migrationBuilder.AddForeignKey(
	        name: "FK_CategoryMaster_CategoryTypeMaster_CategoryTypeMasterId",
	        table: "CategoryMaster",
	        column: "CategoryTypeMasterId",
	        principalTable: "CategoryTypeMaster",
	        principalColumn: "Id",
	        onDelete: ReferentialAction.Cascade);
	}


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryMaster_CategoryTypeMaster_CategoryTypeMasterId",
                table: "CategoryMaster");

            migrationBuilder.DropIndex(
                name: "IX_CategoryMaster_CategoryTypeMasterId",
                table: "CategoryMaster");

            migrationBuilder.RenameColumn(
                name: "CategoryTypeMasterId",
                table: "CategoryMaster",
                newName: "CategoryType");
        }
    }
}

