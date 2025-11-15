using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityManagementApp.Data.Migrations
{
    public partial class AddColorKeyToCategoryTypeMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorKey",
                table: "CategoryTypeMaster",
                type: "TEXT",
                nullable: false,
                defaultValue: "secondary");

            migrationBuilder.AddColumn<string>(
                name: "TextColorKey",
                table: "CategoryTypeMaster",
                type: "TEXT",
                nullable: false,
                defaultValue: "white");
            
            migrationBuilder.Sql(@"
			UPDATE CategoryTypeMaster SET 
			    ColorKey = 
			        CASE TypeName
			            WHEN '生活' THEN 'secondary'
			            WHEN '娯楽' THEN 'warning'
			            WHEN '自己研鑽' THEN 'success'
			            WHEN '睡眠' THEN 'info'
			            WHEN '仕事' THEN 'primary'
			            ELSE 'secondary'
			        END,
			    TextColorKey =
			        CASE TypeName
			            WHEN '娯楽' THEN 'dark'
			            ELSE 'white'
			        END;
			");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorKey",
                table: "CategoryTypeMaster");

            migrationBuilder.DropColumn(
                name: "TextColorKey",
                table: "CategoryTypeMaster");
        }
    }
}
