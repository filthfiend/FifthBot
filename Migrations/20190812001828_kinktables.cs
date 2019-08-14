using Microsoft.EntityFrameworkCore.Migrations;

namespace FifthBot.Migrations
{
    public partial class kinktables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "KinkGroups",
                columns: table => new
                {
                    KinkGroupID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KinkGroupName = table.Column<string>(nullable: true),
                    KinkGroupDescrip = table.Column<string>(nullable: true),
                    KinkMsgID = table.Column<ulong>(nullable: false),
                    LimitMsgID = table.Column<ulong>(nullable: false)
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
                    KinkGroupID = table.Column<ulong>(nullable: false),
                    EmojiName = table.Column<string>(nullable: true),
                    EmojiID = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kinks", x => x.KinkID);
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
                name: "JoinedKinksUsers");

            migrationBuilder.DropTable(
                name: "KinkGroups");

            migrationBuilder.DropTable(
                name: "Kinks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
