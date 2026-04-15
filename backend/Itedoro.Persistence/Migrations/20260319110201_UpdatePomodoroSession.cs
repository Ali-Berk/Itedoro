using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Itedoro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePomodoroSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "ParentSessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PauseStart",
                table: "ParentSessions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PauseStop",
                table: "ParentSessions",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PauseStart",
                table: "ParentSessions");

            migrationBuilder.DropColumn(
                name: "PauseStop",
                table: "ParentSessions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "ParentSessions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
