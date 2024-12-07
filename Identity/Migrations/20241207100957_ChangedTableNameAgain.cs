using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTableNameAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FailedLoginAttempts_users_UserId",
                table: "FailedLoginAttempts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FailedLoginAttempts",
                table: "FailedLoginAttempts");

            migrationBuilder.RenameTable(
                name: "FailedLoginAttempts",
                newName: "failed_login_attempts");

            migrationBuilder.RenameIndex(
                name: "IX_FailedLoginAttempts_UserId",
                table: "failed_login_attempts",
                newName: "IX_failed_login_attempts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_failed_login_attempts",
                table: "failed_login_attempts",
                column: "AttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_failed_login_attempts_users_UserId",
                table: "failed_login_attempts",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_failed_login_attempts_users_UserId",
                table: "failed_login_attempts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_failed_login_attempts",
                table: "failed_login_attempts");

            migrationBuilder.RenameTable(
                name: "failed_login_attempts",
                newName: "FailedLoginAttempts");

            migrationBuilder.RenameIndex(
                name: "IX_failed_login_attempts_UserId",
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
    }
}
