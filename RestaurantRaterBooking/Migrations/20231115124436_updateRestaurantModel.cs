using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantRaterBooking.Migrations
{
    /// <inheritdoc />
    public partial class updateRestaurantModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delivery",
                table: "Restaurant");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Slider",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Restaurant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpecialDish",
                table: "Restaurant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "SpecialDish",
                table: "Restaurant");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Slider",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Delivery",
                table: "Restaurant",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
