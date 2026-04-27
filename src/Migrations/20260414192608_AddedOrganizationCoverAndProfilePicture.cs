using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
	/// <inheritdoc />
	public partial class AddedOrganizationCoverAndProfilePicture : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "CoverPhotoId",
				table: "Organization",
				type: "integer",
				nullable: true);

			migrationBuilder.AddColumn<int>(
				name: "ProfilePictureId",
				table: "Organization",
				type: "integer",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Organization_CoverPhotoId",
				table: "Organization",
				column: "CoverPhotoId");

			migrationBuilder.CreateIndex(
				name: "IX_Organization_ProfilePictureId",
				table: "Organization",
				column: "ProfilePictureId");

			migrationBuilder.AddForeignKey(
				name: "FK_Organization_Attachment_CoverPhotoId",
				table: "Organization",
				column: "CoverPhotoId",
				principalTable: "Attachment",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Organization_Attachment_ProfilePictureId",
				table: "Organization",
				column: "ProfilePictureId",
				principalTable: "Attachment",
				principalColumn: "Id");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Organization_Attachment_CoverPhotoId",
				table: "Organization");

			migrationBuilder.DropForeignKey(
				name: "FK_Organization_Attachment_ProfilePictureId",
				table: "Organization");

			migrationBuilder.DropIndex(
				name: "IX_Organization_CoverPhotoId",
				table: "Organization");

			migrationBuilder.DropIndex(
				name: "IX_Organization_ProfilePictureId",
				table: "Organization");

			migrationBuilder.DropColumn(
				name: "CoverPhotoId",
				table: "Organization");

			migrationBuilder.DropColumn(
				name: "ProfilePictureId",
				table: "Organization");
		}
	}
}