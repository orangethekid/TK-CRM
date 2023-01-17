using Microsoft.EntityFrameworkCore.Migrations;

namespace TakraonlineCRM.Server.Migrations
{
    public partial class Phase2UpdateOrderAddStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "done" );

            migrationBuilder.AddColumn<string>(
                name: "OrderStatusDetail",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderStatusDetail",
                table: "Orders");
        }
    }
}
