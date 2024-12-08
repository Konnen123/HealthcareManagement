using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class RenamedUserIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_admins_staffs_Id",
                table: "admins");

            migrationBuilder.DropForeignKey(
                name: "FK_doctors_staffs_Id",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_users_Id",
                table: "patients");

            migrationBuilder.DropForeignKey(
                name: "FK_staffs_users_Id",
                table: "staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_super_admins_staffs_Id",
                table: "super_admins");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "super_admins",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "staffs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "patients",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "doctors",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "admins",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_admins_staffs_UserId",
                table: "admins",
                column: "UserId",
                principalTable: "staffs",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_staffs_UserId",
                table: "doctors",
                column: "UserId",
                principalTable: "staffs",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patients_users_UserId",
                table: "patients",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_staffs_users_UserId",
                table: "staffs",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_super_admins_staffs_UserId",
                table: "super_admins",
                column: "UserId",
                principalTable: "staffs",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_admins_staffs_UserId",
                table: "admins");

            migrationBuilder.DropForeignKey(
                name: "FK_doctors_staffs_UserId",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_users_UserId",
                table: "patients");

            migrationBuilder.DropForeignKey(
                name: "FK_staffs_users_UserId",
                table: "staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_super_admins_staffs_UserId",
                table: "super_admins");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "super_admins",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "staffs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "patients",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "doctors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "admins",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_admins_staffs_Id",
                table: "admins",
                column: "Id",
                principalTable: "staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_staffs_Id",
                table: "doctors",
                column: "Id",
                principalTable: "staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patients_users_Id",
                table: "patients",
                column: "Id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_staffs_users_Id",
                table: "staffs",
                column: "Id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_super_admins_staffs_Id",
                table: "super_admins",
                column: "Id",
                principalTable: "staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
