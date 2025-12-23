using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDualScoresAndProfilePicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BestResults_Score_AchievedAt",
                table: "BestResults");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "MarathonAttempts",
                newName: "FrontendScore");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "BestResults",
                newName: "BestFrontendScore");

            migrationBuilder.RenameColumn(
                name: "AchievedAt",
                table: "BestResults",
                newName: "FrontendAchievedAt");

            migrationBuilder.AddColumn<int>(
                name: "BackendScore",
                table: "MarathonAttempts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "BackendAchievedAt",
                table: "BestResults",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "BestBackendScore",
                table: "BestResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BestResults_BackendAchievedAt",
                table: "BestResults",
                column: "BackendAchievedAt");

            migrationBuilder.CreateIndex(
                name: "IX_BestResults_BestBackendScore",
                table: "BestResults",
                column: "BestBackendScore");

            migrationBuilder.CreateIndex(
                name: "IX_BestResults_BestFrontendScore",
                table: "BestResults",
                column: "BestFrontendScore");

            migrationBuilder.CreateIndex(
                name: "IX_BestResults_FrontendAchievedAt",
                table: "BestResults",
                column: "FrontendAchievedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BestResults_BackendAchievedAt",
                table: "BestResults");

            migrationBuilder.DropIndex(
                name: "IX_BestResults_BestBackendScore",
                table: "BestResults");

            migrationBuilder.DropIndex(
                name: "IX_BestResults_BestFrontendScore",
                table: "BestResults");

            migrationBuilder.DropIndex(
                name: "IX_BestResults_FrontendAchievedAt",
                table: "BestResults");

            migrationBuilder.DropColumn(
                name: "BackendScore",
                table: "MarathonAttempts");

            migrationBuilder.DropColumn(
                name: "BackendAchievedAt",
                table: "BestResults");

            migrationBuilder.DropColumn(
                name: "BestBackendScore",
                table: "BestResults");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "FrontendScore",
                table: "MarathonAttempts",
                newName: "Score");

            migrationBuilder.RenameColumn(
                name: "FrontendAchievedAt",
                table: "BestResults",
                newName: "AchievedAt");

            migrationBuilder.RenameColumn(
                name: "BestFrontendScore",
                table: "BestResults",
                newName: "Score");

            migrationBuilder.CreateIndex(
                name: "IX_BestResults_Score_AchievedAt",
                table: "BestResults",
                columns: new[] { "Score", "AchievedAt" },
                descending: new[] { true, false });
        }
    }
}
