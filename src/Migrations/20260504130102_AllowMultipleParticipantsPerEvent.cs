using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AllowMultipleParticipantsPerEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserEventBinding_OrganizationEventsId",
                table: "UserEventBinding");

            migrationBuilder.CreateIndex(
                name: "IX_UserEventBinding_OrganizationEventsId",
                table: "UserEventBinding",
                column: "OrganizationEventsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEventBinding_UserId_OrganizationEventsId",
                table: "UserEventBinding",
                columns: new[] { "UserId", "OrganizationEventsId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserEventBinding_OrganizationEventsId",
                table: "UserEventBinding");

            migrationBuilder.DropIndex(
                name: "IX_UserEventBinding_UserId_OrganizationEventsId",
                table: "UserEventBinding");

            migrationBuilder.CreateIndex(
                name: "IX_UserEventBinding_OrganizationEventsId",
                table: "UserEventBinding",
                column: "OrganizationEventsId",
                unique: true);
        }
    }
}
