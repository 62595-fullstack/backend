using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class messages2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    content = table.Column<string>(type: "text", nullable: false),
                    userSenderIdEmail = table.Column<string>(type: "text", nullable: false),
                    userReviverIdEmail = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    userSenderEmail = table.Column<string>(type: "text", nullable: false),
                    userReviverEmail = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_User_userReviverEmail",
                        column: x => x.userReviverEmail,
                        principalTable: "User",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_User_userReviverIdEmail",
                        column: x => x.userReviverIdEmail,
                        principalTable: "User",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_User_userSenderEmail",
                        column: x => x.userSenderEmail,
                        principalTable: "User",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_User_userSenderIdEmail",
                        column: x => x.userSenderIdEmail,
                        principalTable: "User",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_userReviverEmail",
                table: "Message",
                column: "userReviverEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Message_userReviverIdEmail",
                table: "Message",
                column: "userReviverIdEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Message_userSenderEmail",
                table: "Message",
                column: "userSenderEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Message_userSenderIdEmail",
                table: "Message",
                column: "userSenderIdEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");
        }
    }
}
