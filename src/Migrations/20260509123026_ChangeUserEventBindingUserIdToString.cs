using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserEventBindingUserIdToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""UserEventBinding"" ALTER COLUMN ""UserId"" TYPE text USING ""UserId""::text");

            migrationBuilder.Sql(@"ALTER TABLE ""UserEventBinding"" ALTER COLUMN ""UserId"" DROP NOT NULL");

            migrationBuilder.Sql(@"UPDATE ""UserEventBinding"" SET ""UserId"" = NULL WHERE ""UserId"" IS NOT NULL AND ""UserId"" NOT IN (SELECT ""Id"" FROM ""User"")");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEventBinding_User_UserId",
                table: "UserEventBinding",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEventBinding_User_UserId",
                table: "UserEventBinding");

            migrationBuilder.Sql(@"ALTER TABLE ""UserEventBinding"" ALTER COLUMN ""UserId"" TYPE integer USING ""UserId""::integer");
        }
    }
}
