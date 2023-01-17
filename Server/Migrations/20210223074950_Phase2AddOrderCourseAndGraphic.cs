using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TakraonlineCRM.Server.Migrations
{
    public partial class Phase2AddOrderCourseAndGraphic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Graphics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    DraftDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FeedBack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DraftFile = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Graphics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderGraphics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FocusDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DraftDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderGraphics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderCourses_OrderId",
                table: "OrderCourses",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderGraphics_OrderId",
                table: "OrderGraphics",
                column: "OrderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderCourses_Orders_OrderId",
                table: "OrderCourses");

            migrationBuilder.DropTable(
                name: "Graphics");

            migrationBuilder.DropTable(
                name: "OrderGraphics");

            migrationBuilder.DropIndex(
                name: "IX_OrderCourses_OrderId",
                table: "OrderCourses");
        }
    }
}
