using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
	/// <inheritdoc />
	public partial class RenameImageUrlIdToAttachmentId : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_OrganizationEvent_Attachment_ImageUrlId",
				table: "OrganizationEvent");

			migrationBuilder.RenameColumn(
				name: "ImageUrlId",
				table: "OrganizationEvent",
				newName: "AttachmentId");

			migrationBuilder.RenameIndex(
				name: "IX_OrganizationEvent_ImageUrlId",
				table: "OrganizationEvent",
				newName: "IX_OrganizationEvent_AttachmentId");

			migrationBuilder.AddForeignKey(
				name: "FK_OrganizationEvent_Attachment_AttachmentId",
				table: "OrganizationEvent",
				column: "AttachmentId",
				principalTable: "Attachment",
				principalColumn: "Id");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_OrganizationEvent_Attachment_AttachmentId",
				table: "OrganizationEvent");

			migrationBuilder.RenameColumn(
				name: "AttachmentId",
				table: "OrganizationEvent",
				newName: "ImageUrlId");

			migrationBuilder.RenameIndex(
				name: "IX_OrganizationEvent_AttachmentId",
				table: "OrganizationEvent",
				newName: "IX_OrganizationEvent_ImageUrlId");

			migrationBuilder.AddForeignKey(
				name: "FK_OrganizationEvent_Attachment_ImageUrlId",
				table: "OrganizationEvent",
				column: "ImageUrlId",
				principalTable: "Attachment",
				principalColumn: "Id");
		}
	}
}