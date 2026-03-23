using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddAttachmentToOrganizationEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Post_PostId",
                table: "Attachment");

            migrationBuilder.AlterColumn<int>(
                name: "UserOrganizationBindingId",
                table: "OrganizationEvent",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "OrganizationEvent",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageUrlId",
                table: "OrganizationEvent",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Attachment",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEvent_ImageUrlId",
                table: "OrganizationEvent",
                column: "ImageUrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Post_PostId",
                table: "Attachment",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationEvent_Attachment_ImageUrlId",
                table: "OrganizationEvent",
                column: "ImageUrlId",
                principalTable: "Attachment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Post_PostId",
                table: "Attachment");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationEvent_Attachment_ImageUrlId",
                table: "OrganizationEvent");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationEvent_ImageUrlId",
                table: "OrganizationEvent");

            migrationBuilder.DropColumn(
                name: "ImageUrlId",
                table: "OrganizationEvent");

            migrationBuilder.AlterColumn<int>(
                name: "UserOrganizationBindingId",
                table: "OrganizationEvent",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "OrganizationEvent",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "OrganizationEvent",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Attachment",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Post_PostId",
                table: "Attachment",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
