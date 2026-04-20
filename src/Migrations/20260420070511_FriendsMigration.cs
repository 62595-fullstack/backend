using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

#nullable disable

namespace backend.Migrations
{
	/// <inheritdoc />
	public partial class FriendsMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddUniqueConstraint(
				name: "AK_User_Id",
				table: "User",
				column: "Id");

			migrationBuilder.CreateTable(
				name: "UserFriendship",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					UserAId = table.Column<string>(type: "text", nullable: false),
					UserBId = table.Column<string>(type: "text", nullable: false),
					CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserFriendship", x => x.Id);
					table.ForeignKey(
						name: "FK_UserFriendship_User_UserAId",
						column: x => x.UserAId,
						principalTable: "User",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_UserFriendship_User_UserBId",
						column: x => x.UserBId,
						principalTable: "User",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_UserFriendship_UserAId_UserBId",
				table: "UserFriendship",
				columns: new[] { "UserAId", "UserBId" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_UserFriendship_UserBId",
				table: "UserFriendship",
				column: "UserBId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "UserFriendship");

			migrationBuilder.DropUniqueConstraint(
				name: "AK_User_Id",
				table: "User");
		}
	}
}