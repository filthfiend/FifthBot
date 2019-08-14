using Microsoft.EntityFrameworkCore.Migrations;

namespace FifthBot.Migrations
{
    public partial class fiddling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Commands",
                newName: "CommandName");

            migrationBuilder.RenameColumn(
                name: "MsgID",
                table: "Commands",
                newName: "CommandID");

            migrationBuilder.AddColumn<ulong>(
                name: "ChannelID",
                table: "Commands",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<string>(
                name: "CommandData",
                table: "Commands",
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "MessageID",
                table: "Commands",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChannelID",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "CommandData",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "MessageID",
                table: "Commands");

            migrationBuilder.RenameColumn(
                name: "CommandName",
                table: "Commands",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CommandID",
                table: "Commands",
                newName: "MsgID");
        }
    }
}
