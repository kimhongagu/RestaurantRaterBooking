using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantRaterBooking.Migrations
{
    /// <inheritdoc />
    public partial class CreateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningHour = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClosingHour = table.Column<TimeSpan>(type: "time", nullable: false),
                    SuitableFor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialFeature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Space = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParkingSpace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChildrenChair = table.Column<bool>(type: "bit", nullable: false),
                    Wifi = table.Column<bool>(type: "bit", nullable: false),
                    AirConditioning = table.Column<bool>(type: "bit", nullable: false),
                    VisaMasterCard = table.Column<bool>(type: "bit", nullable: false),
                    VATInvoice = table.Column<bool>(type: "bit", nullable: false),
                    PrivateRoom = table.Column<bool>(type: "bit", nullable: false),
                    OutdoorTable = table.Column<bool>(type: "bit", nullable: false),
                    ChildrenPlayArea = table.Column<bool>(type: "bit", nullable: false),
                    SmokiingArea = table.Column<bool>(type: "bit", nullable: false),
                    DirectBill = table.Column<bool>(type: "bit", nullable: false),
                    Delivery = table.Column<bool>(type: "bit", nullable: false),
                    Karaoke = table.Column<bool>(type: "bit", nullable: false),
                    Projector = table.Column<bool>(type: "bit", nullable: false),
                    EnventDecoration = table.Column<bool>(type: "bit", nullable: false),
                    CityID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restaurant_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Restaurant_City_CityID",
                        column: x => x.CityID,
                        principalTable: "City",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Slider",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slider_City_CityID",
                        column: x => x.CityID,
                        principalTable: "City",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false),
                    IsHot = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EđitedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostCategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blog_PostCategory_PostCategoryID",
                        column: x => x.PostCategoryID,
                        principalTable: "PostCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false),
                    IsHot = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EđitedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostCategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_PostCategory_PostCategoryID",
                        column: x => x.PostCategoryID,
                        principalTable: "PostCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Adults = table.Column<int>(type: "int", nullable: false),
                    Children = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepositStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RestaurantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_Restaurant_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_Users_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestaurantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Restaurant_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
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

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RestaurantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_Restaurant_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Review_Users_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Blog_PostCategoryID",
                table: "Blog",
                column: "PostCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_BlogTag_BlogID",
                table: "BlogTag",
                column: "BlogID");

            migrationBuilder.CreateIndex(
                name: "IX_BlogTag_TagID",
                table: "BlogTag",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ApplicationUserId",
                table: "Booking",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_RestaurantID",
                table: "Booking",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Image_RestaurantID",
                table: "Image",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_News_PostCategoryID",
                table: "News",
                column: "PostCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsTag_NewsID",
                table: "NewsTag",
                column: "NewsID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsTag_TagID",
                table: "NewsTag",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_CategoryID",
                table: "Restaurant",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_CityID",
                table: "Restaurant",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantTag_RestaurantID",
                table: "RestaurantTag",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantTag_TagID",
                table: "RestaurantTag",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ApplicationUserId",
                table: "Review",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_RestaurantID",
                table: "Review",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Slider_CityID",
                table: "Slider",
                column: "CityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogTag");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "NewsTag");

            migrationBuilder.DropTable(
                name: "RestaurantTag");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Slider");

            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Restaurant");

            migrationBuilder.DropTable(
                name: "PostCategory");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "City");
        }
    }
}
