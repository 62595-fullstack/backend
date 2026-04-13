using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
	/// <inheritdoc />
	public partial class MakeEmailAUniqueIdentifier : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Post_User_UserId1",
				table: "Post");

			migrationBuilder.DropForeignKey(
				name: "FK_UserEventBinding_User_UserId1",
				table: "UserEventBinding");

			migrationBuilder.DropForeignKey(
				name: "FK_UserOrganizationBinding_User_UserId1",
				table: "UserOrganizationBinding");

			migrationBuilder.DropPrimaryKey(
				name: "PK_User",
				table: "User");

			migrationBuilder.RenameColumn(
				name: "UserId1",
				table: "UserOrganizationBinding",
				newName: "UserEmail");

			migrationBuilder.RenameIndex(
				name: "IX_UserOrganizationBinding_UserId1",
				table: "UserOrganizationBinding",
				newName: "IX_UserOrganizationBinding_UserEmail");

			migrationBuilder.RenameColumn(
				name: "UserId1",
				table: "UserEventBinding",
				newName: "UserEmail");

			migrationBuilder.RenameIndex(
				name: "IX_UserEventBinding_UserId1",
				table: "UserEventBinding",
				newName: "IX_UserEventBinding_UserEmail");

			migrationBuilder.RenameColumn(
				name: "UserId1",
				table: "Post",
				newName: "UserEmail");

			migrationBuilder.RenameIndex(
				name: "IX_Post_UserId1",
				table: "Post",
				newName: "IX_Post_UserEmail");

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "User",
				type: "text",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "text",
				oldNullable: true);

			migrationBuilder.AddPrimaryKey(
				name: "PK_User",
				table: "User",
				column: "Email");

			migrationBuilder.AddForeignKey(
				name: "FK_Post_User_UserEmail",
				table: "Post",
				column: "UserEmail",
				principalTable: "User",
				principalColumn: "Email");

			migrationBuilder.AddForeignKey(
				name: "FK_UserEventBinding_User_UserEmail",
				table: "UserEventBinding",
				column: "UserEmail",
				principalTable: "User",
				principalColumn: "Email");

			migrationBuilder.AddForeignKey(
				name: "FK_UserOrganizationBinding_User_UserEmail",
				table: "UserOrganizationBinding",
				column: "UserEmail",
				principalTable: "User",
				principalColumn: "Email");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Post_User_UserEmail",
				table: "Post");

			migrationBuilder.DropForeignKey(
				name: "FK_UserEventBinding_User_UserEmail",
				table: "UserEventBinding");

			migrationBuilder.DropForeignKey(
				name: "FK_UserOrganizationBinding_User_UserEmail",
				table: "UserOrganizationBinding");

			migrationBuilder.DropPrimaryKey(
				name: "PK_User",
				table: "User");

			migrationBuilder.RenameColumn(
				name: "UserEmail",
				table: "UserOrganizationBinding",
				newName: "UserId1");

			migrationBuilder.RenameIndex(
				name: "IX_UserOrganizationBinding_UserEmail",
				table: "UserOrganizationBinding",
				newName: "IX_UserOrganizationBinding_UserId1");

			migrationBuilder.RenameColumn(
				name: "UserEmail",
				table: "UserEventBinding",
				newName: "UserId1");

			migrationBuilder.RenameIndex(
				name: "IX_UserEventBinding_UserEmail",
				table: "UserEventBinding",
				newName: "IX_UserEventBinding_UserId1");

			migrationBuilder.RenameColumn(
				name: "UserEmail",
				table: "Post",
				newName: "UserId1");

			migrationBuilder.RenameIndex(
				name: "IX_Post_UserEmail",
				table: "Post",
				newName: "IX_Post_UserId1");

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "User",
				type: "text",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "text");

			migrationBuilder.AddPrimaryKey(
				name: "PK_User",
				table: "User",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Post_User_UserId1",
				table: "Post",
				column: "UserId1",
				principalTable: "User",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_UserEventBinding_User_UserId1",
				table: "UserEventBinding",
				column: "UserId1",
				principalTable: "User",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_UserOrganizationBinding_User_UserId1",
				table: "UserOrganizationBinding",
				column: "UserId1",
				principalTable: "User",
				principalColumn: "Id");
		}
	}
}