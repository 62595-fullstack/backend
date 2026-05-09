using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
	[DbContext(typeof(DatabaseContext))]
	[Migration("20260427000000_AddEventRulesAndBracketResults")]
	public partial class AddEventRulesAndBracketResults : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "BracketResults",
				table: "OrganizationEvent",
				type: "text",
				nullable: false,
				defaultValue: "{}");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "BracketResults",
				table: "OrganizationEvent");
		}
	}
}
