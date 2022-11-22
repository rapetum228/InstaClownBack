using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class FixComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ResponseCommentId",
                table: "Comments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ResponseCommentId",
                table: "Comments",
                column: "ResponseCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ResponseCommentId",
                table: "Comments",
                column: "ResponseCommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ResponseCommentId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ResponseCommentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ResponseCommentId",
                table: "Comments");
        }
    }
}
