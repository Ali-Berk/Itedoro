using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Itedoro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserStatsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTotalStats",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalCompletedPomodoros = table.Column<int>(type: "integer", nullable: false),
                    TotalStudyTimeInMinutes = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTotalStats", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserWeekStats",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    WeekId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CompletedPomodoros = table.Column<int>(type: "integer", nullable: false),
                    PlannedPomodoros = table.Column<int>(type: "integer", nullable: false),
                    CompletedPlans = table.Column<int>(type: "integer", nullable: false),
                    PlannedPlans = table.Column<int>(type: "integer", nullable: false),
                    WeeklyStudyTimeInMinutes = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWeekStats", x => new { x.UserId, x.WeekId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTotalStats_UserId",
                table: "UserTotalStats",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserWeekStats_UserId_WeekId",
                table: "UserWeekStats",
                columns: new[] { "UserId", "WeekId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTotalStats");

            migrationBuilder.DropTable(
                name: "UserWeekStats");
        }
    }
}
