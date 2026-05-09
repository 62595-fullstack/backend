using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
	[DbContext(typeof(DatabaseContext))]
	[Migration("20260429000000_EnsureEventRulesColumn")]
	public partial class EnsureEventRulesColumn : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "Rules",
				table: "OrganizationEvent",
				type: "text",
				nullable: false,
				defaultValue: "");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Rules",
				table: "OrganizationEvent");
		}
	}
}
