using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMobdevTrack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MobdevScore",
                table: "MarathonAttempts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BestMobdevScore",
                table: "BestResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "MobdevAchievedAt",
                table: "BestResults",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_BestResults_BestMobdevScore",
                table: "BestResults",
                column: "BestMobdevScore");

            migrationBuilder.CreateIndex(
                name: "IX_BestResults_MobdevAchievedAt",
                table: "BestResults",
                column: "MobdevAchievedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BestResults_BestMobdevScore",
                table: "BestResults");

            migrationBuilder.DropIndex(
                name: "IX_BestResults_MobdevAchievedAt",
                table: "BestResults");

            migrationBuilder.DropColumn(
                name: "MobdevScore",
                table: "MarathonAttempts");

            migrationBuilder.DropColumn(
                name: "BestMobdevScore",
                table: "BestResults");

            migrationBuilder.DropColumn(
                name: "MobdevAchievedAt",
                table: "BestResults");
        }
    }
}
