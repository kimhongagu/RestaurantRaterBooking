using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantRaterBooking.Migrations
{
    /// <inheritdoc />
    public partial class updateBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Users_ApplicationUserId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_ApplicationUserId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "DepositAmount",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "DepositStatus",
                table: "Booking");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Booking",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Booking",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Children",
                table: "Booking",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Adults",
                table: "Booking",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UserID",
                table: "Booking",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Users_UserID",
                table: "Booking",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Users_UserID",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_UserID",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Booking",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Children",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Adults",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Booking",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DepositAmount",
                table: "Booking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DepositStatus",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ApplicationUserId",
                table: "Booking",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Users_ApplicationUserId",
                table: "Booking",
                column: "ApplicationUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
