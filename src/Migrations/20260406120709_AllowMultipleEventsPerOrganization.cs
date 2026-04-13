using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
	/// <inheritdoc />
	public partial class AllowMultipleEventsPerOrganization : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_OrganizationEvent_OrganizationId",
				table: "OrganizationEvent");

			migrationBuilder.CreateIndex(
				name: "IX_OrganizationEvent_OrganizationId",
				table: "OrganizationEvent",
				column: "OrganizationId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_OrganizationEvent_OrganizationId",
				table: "OrganizationEvent");

			migrationBuilder.CreateIndex(
				name: "IX_OrganizationEvent_OrganizationId",
				table: "OrganizationEvent",
				column: "OrganizationId",
				unique: true);
		}
	}
}