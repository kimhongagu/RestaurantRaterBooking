using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantRaterBooking.Migrations
{
    /// <inheritdoc />
    public partial class addReplyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reply",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReplyComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReplyAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reply_Review_ReviewID",
                        column: x => x.ReviewID,
                        principalTable: "Review",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reply_ReviewID",
                table: "Reply",
                column: "ReviewID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reply");
        }
    }
}
