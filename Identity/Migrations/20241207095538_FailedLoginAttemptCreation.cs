using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    /// <inheritdoc />
    public partial class FailedLoginAttemptCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FailedLoginAttempt",
                columns: table => new
                {
                    AttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FailedAttempts = table.Column<int>(type: "integer", nullable: false),
                    LastFailedAttemptTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LockoutEndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MaxFailedLoginAttempts = table.Column<int>(type: "integer", nullable: false, defaultValue: 5)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedLoginAttempt", x => x.AttemptId);
                    table.ForeignKey(
                        name: "FK_FailedLoginAttempt_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FailedLoginAttempt_UserId",
                table: "FailedLoginAttempt",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FailedLoginAttempt");
        }
    }
}
