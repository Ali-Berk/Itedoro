using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Itedoro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderToChildSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ChildSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "ChildSessions");
        }
    }
}
