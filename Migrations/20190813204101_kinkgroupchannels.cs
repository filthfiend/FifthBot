using Microsoft.EntityFrameworkCore.Migrations;

namespace FifthBot.Migrations
{
    public partial class kinkgroupchannels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "KinkChannelID",
                table: "KinkGroups",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "LimitChannelID",
                table: "KinkGroups",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<int>(
                name: "CommandStep",
                table: "Commands",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KinkChannelID",
                table: "KinkGroups");

            migrationBuilder.DropColumn(
                name: "LimitChannelID",
                table: "KinkGroups");

            migrationBuilder.DropColumn(
                name: "CommandStep",
                table: "Commands");
        }
    }
}
