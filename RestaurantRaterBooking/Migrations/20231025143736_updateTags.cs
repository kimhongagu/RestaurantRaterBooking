using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantRaterBooking.Migrations
{
    /// <inheritdoc />
    public partial class updateTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogTag");

            migrationBuilder.DropTable(
                name: "NewsTag");

            migrationBuilder.DropTable(
                name: "RestaurantTag");

            migrationBuilder.AddColumn<Guid>(
                name: "BlogId",
                table: "Tag",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NewsId",
                table: "Tag",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RestaurantId",
                table: "Tag",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeTag",
                table: "Tag",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_BlogId",
                table: "Tag",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_NewsId",
                table: "Tag",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_RestaurantId",
                table: "Tag",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Blog_BlogId",
                table: "Tag",
                column: "BlogId",
                principalTable: "Blog",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_News_NewsId",
                table: "Tag",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Restaurant_RestaurantId",
                table: "Tag",
                column: "RestaurantId",
                principalTable: "Restaurant",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Blog_BlogId",
                table: "Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_News_NewsId",
                table: "Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Restaurant_RestaurantId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_BlogId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_NewsId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_RestaurantId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "NewsId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "TypeTag",
                table: "Tag");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Image",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BlogTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TagID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogTag_Blog_BlogID",
                        column: x => x.BlogID,
                        principalTable: "Blog",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BlogTag_Tag_TagID",
                        column: x => x.TagID,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewsID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TagID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsTag_News_NewsID",
                        column: x => x.NewsID,
                        principalTable: "News",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NewsTag_Tag_TagID",
                        column: x => x.TagID,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RestaurantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TagID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantTag_Restaurant_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RestaurantTag_Tag_TagID",
                        column: x => x.TagID,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogTag_BlogID",
                table: "BlogTag",
                column: "BlogID");

            migrationBuilder.CreateIndex(
                name: "IX_BlogTag_TagID",
                table: "BlogTag",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsTag_NewsID",
                table: "NewsTag",
                column: "NewsID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsTag_TagID",
                table: "NewsTag",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantTag_RestaurantID",
                table: "RestaurantTag",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantTag_TagID",
                table: "RestaurantTag",
                column: "TagID");
        }
    }
}
