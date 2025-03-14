using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueenOfApostlesRenewalCentre.Migrations
{
    /// <inheritdoc />
    public partial class AddMultipleRoomsSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomIds",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 1,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 2,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 3,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 4,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 5,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 6,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 7,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 8,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 9,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 10,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 11,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 12,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 13,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 14,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 15,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 16,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 17,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 18,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 19,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 20,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 21,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 22,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 23,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 24,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 25,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 26,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 27,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 28,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 29,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 30,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 31,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 32,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 33,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 34,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 35,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 36,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 37,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 38,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 39,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 40,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 41,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 42,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 43,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 44,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 45,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 46,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 47,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 48,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 49,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 50,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 51,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 52,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 53,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 54,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 55,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 56,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 57,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 58,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 59,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 60,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 61,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 62,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 63,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 64,
                column: "BookingId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BookingId",
                table: "Rooms",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Bookings_BookingId",
                table: "Rooms",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "BookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Bookings_BookingId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_BookingId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomIds",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
