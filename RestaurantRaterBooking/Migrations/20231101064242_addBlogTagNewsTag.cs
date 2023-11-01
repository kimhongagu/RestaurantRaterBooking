using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantRaterBooking.Migrations
{
    /// <inheritdoc />
    public partial class addBlogTagNewsTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Tag_Blog_BlogId",
            //    table: "Tag");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Tag_News_NewsId",
            //    table: "Tag");

            //migrationBuilder.DropIndex(
            //    name: "IX_Tag_BlogId",
            //    table: "Tag");

            //migrationBuilder.DropIndex(
            //    name: "IX_Tag_NewsId",
            //    table: "Tag");

            //migrationBuilder.DropColumn(
            //    name: "BlogId",
            //    table: "Tag");

            //migrationBuilder.DropColumn(
            //    name: "NewsId",
            //    table: "Tag");

            migrationBuilder.AddColumn<int>(
                name: "TagType",
                table: "Tag",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BlogTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogTag_Blog_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsTag_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogTag_BlogId",
                table: "BlogTag",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogTag_TagId",
                table: "BlogTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsTag_NewsId",
                table: "NewsTag",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsTag_TagId",
                table: "NewsTag",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogTag");

            migrationBuilder.DropTable(
                name: "NewsTag");

            migrationBuilder.DropColumn(
                name: "TagType",
                table: "Tag");

            migrationBuilder.RenameColumn(
                name: "EditedAt",
                table: "News",
                newName: "EđitedAt");

            migrationBuilder.RenameColumn(
                name: "EditedAt",
                table: "Blog",
                newName: "EđitedAt");

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

            migrationBuilder.CreateIndex(
                name: "IX_Tag_BlogId",
                table: "Tag",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_NewsId",
                table: "Tag",
                column: "NewsId");

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
        }
    }
}
