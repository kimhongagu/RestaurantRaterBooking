using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantRaterBooking.Migrations
{
    /// <inheritdoc />
    public partial class updateReview2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Review_Users_ApplicationUserId",
            //    table: "Review");

            //migrationBuilder.DropIndex(
            //    name: "IX_Review_ApplicationUserId",
            //    table: "Review");

            //migrationBuilder.DropColumn(
            //    name: "ApplicationUserId",
            //    table: "Review");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Review",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Review_UserID",
            //    table: "Review",
            //    column: "UserID");

            //    migrationBuilder.AddForeignKey(
            //        name: "FK_Review_Users_UserID",
            //        table: "Review",
            //        column: "UserID",
            //        principalTable: "Users",
            //        principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Users_UserID",
                table: "Review");

            //migrationBuilder.DropIndex(
            //    name: "IX_Review_UserID",
            //    table: "Review");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Review",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Review",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Review_ApplicationUserId",
                table: "Review",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Users_ApplicationUserId",
                table: "Review",
                column: "ApplicationUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
