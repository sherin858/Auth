using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Authentication.Migrations
{
    /// <inheritdoc />
    public partial class RenamingClaimsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Employees_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "EmployesClaims");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "EmployesClaims",
                newName: "IX_EmployesClaims_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployesClaims",
                table: "EmployesClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployesClaims_Employees_UserId",
                table: "EmployesClaims",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployesClaims_Employees_UserId",
                table: "EmployesClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployesClaims",
                table: "EmployesClaims");

            migrationBuilder.RenameTable(
                name: "EmployesClaims",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameIndex(
                name: "IX_EmployesClaims_UserId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Employees_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
