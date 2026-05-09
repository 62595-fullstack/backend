using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserOrganizationBindingUserIdToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""UserOrganizationBinding"" ALTER COLUMN ""UserId"" TYPE text USING ""UserId""::text");

            migrationBuilder.Sql(@"UPDATE ""UserOrganizationBinding"" SET ""UserId"" = NULL WHERE ""UserId"" IS NOT NULL AND ""UserId"" NOT IN (SELECT ""Id"" FROM ""User"")");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganizationBinding_User_UserId",
                table: "UserOrganizationBinding",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganizationBinding_User_UserId",
                table: "UserOrganizationBinding");

            migrationBuilder.Sql(@"ALTER TABLE ""UserOrganizationBinding"" ALTER COLUMN ""UserId"" TYPE integer USING ""UserId""::integer");
        }
    }
}
