using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantRaterBooking.Migrations
{
    /// <inheritdoc />
    public partial class addRestaurantTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Restaurant_RestaurantId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_RestaurantId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Tag");

            migrationBuilder.CreateTable(
                name: "RestaurantTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RestaurantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantTag_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RestaurantTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantTag_RestaurantId",
                table: "RestaurantTag",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantTag_TagId",
                table: "RestaurantTag",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantTag");

            migrationBuilder.AddColumn<Guid>(
                name: "RestaurantId",
                table: "Tag",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_RestaurantId",
                table: "Tag",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Restaurant_RestaurantId",
                table: "Tag",
                column: "RestaurantId",
                principalTable: "Restaurant",
                principalColumn: "Id");
        }
    }
}
