using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Itedoro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveTimeTrackingToParent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "ChildSessions");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "ChildSessions");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ParentSessions",
                newName: "StartTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "ParentSessions",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "ParentSessions");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "ParentSessions",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "ChildSessions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "ChildSessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
