using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantRaterBooking.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeTag",
                table: "Tag");

			migrationBuilder.DropColumn(
				name: "BlogId",
				table: "Tag");

			migrationBuilder.DropColumn(
				name: "NewsId",
				table: "Tag");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeTag",
                table: "Tag",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
