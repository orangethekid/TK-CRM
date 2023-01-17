using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TakraonlineCRM.Server.Migrations
{
    public partial class Phase2AddActivitiesLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "orderId",
                table: "WebSiteProgramEdits",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "orderId",
                table: "WebSiteNewDesigns",
                newName: "OrderId");

            migrationBuilder.CreateTable(
                name: "ActivitiesLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Actionlog = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackupObject = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitiesLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivitiesLogs");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "WebSiteProgramEdits",
                newName: "orderId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "WebSiteNewDesigns",
                newName: "orderId");
        }
    }
}
