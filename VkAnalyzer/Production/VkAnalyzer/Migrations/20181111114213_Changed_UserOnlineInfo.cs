using Microsoft.EntityFrameworkCore.Migrations;

namespace VkAnalyzer.Migrations
{
    public partial class Changed_UserOnlineInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "UserOnlineInfos",
                newName: "DateTime");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "UserOnlineInfos",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "UserOnlineInfos",
                newName: "Time");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserOnlineInfos",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
