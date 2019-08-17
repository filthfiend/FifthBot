using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FifthBot.Migrations
{
    public partial class onceagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attacks",
                columns: table => new
                {
                    MessageID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AttackerId = table.Column<ulong>(nullable: false),
                    TargetId = table.Column<ulong>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    DateandTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attacks", x => x.MessageID);
                });

            migrationBuilder.CreateTable(
                name: "Commands",
                columns: table => new
                {
                    CommandID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageID = table.Column<ulong>(nullable: false),
                    ActorID = table.Column<ulong>(nullable: false),
                    TargetID = table.Column<ulong>(nullable: false),
                    ChannelID = table.Column<ulong>(nullable: false),
                    CommandName = table.Column<string>(nullable: true),
                    CommandData = table.Column<string>(nullable: true),
                    CommandStep = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commands", x => x.CommandID);
                });

            migrationBuilder.CreateTable(
                name: "IntroChannels",
                columns: table => new
                {
                    ChannelID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServerID = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntroChannels", x => x.ChannelID);
                });

            migrationBuilder.CreateTable(
                name: "JoinedKinksUsers",
                columns: table => new
                {
                    JoinID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KinkID = table.Column<ulong>(nullable: false),
                    UserID = table.Column<ulong>(nullable: false),
                    IsLimit = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinedKinksUsers", x => x.JoinID);
                });

            migrationBuilder.CreateTable(
                name: "KinkEmojis",
                columns: table => new
                {
                    JoinID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KinkID = table.Column<ulong>(nullable: false),
                    ServerID = table.Column<ulong>(nullable: false),
                    EmojiName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KinkEmojis", x => x.JoinID);
                });

            migrationBuilder.CreateTable(
                name: "KinkGroupMenus",
                columns: table => new
                {
                    JoinID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KinkGroupID = table.Column<ulong>(nullable: false),
                    ServerID = table.Column<ulong>(nullable: false),
                    KinkMsgID = table.Column<ulong>(nullable: false),
                    KinkChannelID = table.Column<ulong>(nullable: false),
                    LimitMsgID = table.Column<ulong>(nullable: false),
                    LimitChannelID = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KinkGroupMenus", x => x.JoinID);
                });

            migrationBuilder.CreateTable(
                name: "KinkGroups",
                columns: table => new
                {
                    KinkGroupID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KinkGroupName = table.Column<string>(nullable: true),
                    KinkGroupDescrip = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KinkGroups", x => x.KinkGroupID);
                });

            migrationBuilder.CreateTable(
                name: "Kinks",
                columns: table => new
                {
                    KinkID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KinkName = table.Column<string>(nullable: true),
                    KinkDesc = table.Column<string>(nullable: true),
                    KinkGroupID = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kinks", x => x.KinkID);
                });

            migrationBuilder.CreateTable(
                name: "ServerSettings",
                columns: table => new
                {
                    ServerID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerSettings", x => x.ServerID);
                });

            migrationBuilder.CreateTable(
                name: "Stones",
                columns: table => new
                {
                    UserId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stones", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServerID = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attacks");

            migrationBuilder.DropTable(
                name: "Commands");

            migrationBuilder.DropTable(
                name: "IntroChannels");

            migrationBuilder.DropTable(
                name: "JoinedKinksUsers");

            migrationBuilder.DropTable(
                name: "KinkEmojis");

            migrationBuilder.DropTable(
                name: "KinkGroupMenus");

            migrationBuilder.DropTable(
                name: "KinkGroups");

            migrationBuilder.DropTable(
                name: "Kinks");

            migrationBuilder.DropTable(
                name: "ServerSettings");

            migrationBuilder.DropTable(
                name: "Stones");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
