using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class FixUserOrganizationBindingUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserOrganizationBinding_OrganizationId",
                table: "UserOrganizationBinding");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizationBinding_UserId_OrganizationId",
                table: "UserOrganizationBinding",
                columns: new[] { "UserId", "OrganizationId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserOrganizationBinding_UserId_OrganizationId",
                table: "UserOrganizationBinding");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizationBinding_OrganizationId",
                table: "UserOrganizationBinding",
                column: "OrganizationId",
                unique: true);
        }
    }
}
