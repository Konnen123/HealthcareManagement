using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    /// <inheritdoc />
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
    "SonarQube",
    "SXXXX",
    Justification = "Excluded migration file from Sonar analysis.")]
    public partial class ChangedTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FailedLoginAttempt_users_UserId",
                table: "FailedLoginAttempt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FailedLoginAttempt",
                table: "FailedLoginAttempt");

            migrationBuilder.RenameTable(
                name: "FailedLoginAttempt",
                newName: "FailedLoginAttempts");

            migrationBuilder.RenameIndex(
                name: "IX_FailedLoginAttempt_UserId",
                table: "FailedLoginAttempts",
                newName: "IX_FailedLoginAttempts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FailedLoginAttempts",
                table: "FailedLoginAttempts",
                column: "AttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_FailedLoginAttempts_users_UserId",
                table: "FailedLoginAttempts",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FailedLoginAttempts_users_UserId",
                table: "FailedLoginAttempts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FailedLoginAttempts",
                table: "FailedLoginAttempts");

            migrationBuilder.RenameTable(
                name: "FailedLoginAttempts",
                newName: "FailedLoginAttempt");

            migrationBuilder.RenameIndex(
                name: "IX_FailedLoginAttempts_UserId",
                table: "FailedLoginAttempt",
                newName: "IX_FailedLoginAttempt_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FailedLoginAttempt",
                table: "FailedLoginAttempt",
                column: "AttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_FailedLoginAttempt_users_UserId",
                table: "FailedLoginAttempt",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
