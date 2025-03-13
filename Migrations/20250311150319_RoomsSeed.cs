using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QueenOfApostlesRenewalCentre.Migrations
{
    /// <inheritdoc />
    public partial class RoomsSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "Capacity", "IsReserved", "Name", "RoomNumber", "Type", "WithShower" },
                values: new object[,]
                {
                    { 1, 2, false, "Room 1", "1", "Standard", true },
                    { 2, 2, false, "Room 2", "2", "Standard", true },
                    { 3, 2, false, "Room 3", "3", "Standard", true },
                    { 4, 2, false, "Room 4", "4", "Standard", true },
                    { 5, 2, false, "Room 5", "5", "Standard", true },
                    { 6, 2, false, "Room 6", "6", "Standard", true },
                    { 7, 2, false, "Room 7", "7", "Standard", true },
                    { 8, 2, false, "Room 8", "8", "Standard", true },
                    { 9, 2, false, "Room 9", "9", "Standard", true },
                    { 10, 2, false, "Room 10", "10", "Standard", true },
                    { 11, 2, false, "Room 11", "11", "Standard", true },
                    { 12, 2, false, "Room 12", "12", "Standard", true },
                    { 13, 2, false, "Room 13", "13", "Standard", true },
                    { 14, 2, false, "Room 14", "14", "Standard", true },
                    { 15, 2, false, "Room 15", "15", "Standard", true },
                    { 16, 2, false, "Room 16", "16", "Standard", true },
                    { 17, 2, false, "Room 17", "17", "Standard", true },
                    { 18, 2, false, "Room 18", "18", "Standard", true },
                    { 19, 2, false, "Room 19", "19", "Standard", true },
                    { 20, 2, false, "Room 20", "20", "Standard", true },
                    { 21, 2, false, "Room 21", "21", "Standard", true },
                    { 22, 2, false, "Room 22", "22", "Standard", true },
                    { 23, 2, false, "Room 23", "23", "Standard", true },
                    { 24, 2, false, "Room 24", "24", "Standard", true },
                    { 25, 2, false, "Room 25", "25", "Standard", true },
                    { 26, 2, false, "Room 26", "26", "Standard", true },
                    { 27, 2, false, "Room 27", "27", "Standard", true },
                    { 28, 2, false, "Room 28", "28", "Standard", true },
                    { 29, 2, false, "Room 29", "29", "Standard", true },
                    { 30, 2, false, "Room 30", "30", "Standard", true },
                    { 31, 2, false, "Room 31", "31", "Standard", true },
                    { 32, 2, false, "Room 32", "32", "Standard", true },
                    { 33, 2, false, "Room 33", "33", "Standard", true },
                    { 34, 2, false, "Room 34", "34", "Standard", true },
                    { 35, 2, false, "Room 35", "35", "Standard", true },
                    { 36, 2, false, "Room 36", "36", "Standard", true },
                    { 37, 2, false, "Room 37", "37", "Standard", true },
                    { 38, 2, false, "Room 38", "38", "Standard", true },
                    { 39, 2, false, "Room 39", "39", "Standard", true },
                    { 40, 2, false, "Room 40", "40", "Standard", true },
                    { 41, 2, false, "Room 41", "41", "Standard", true },
                    { 42, 2, false, "Room 42", "42", "Standard", true },
                    { 43, 2, false, "Room 43", "43", "Standard", true },
                    { 44, 2, false, "Room 44", "44", "Standard", true },
                    { 45, 2, false, "Room 45", "45", "Standard", true },
                    { 46, 2, false, "Room 46", "46", "Standard", true },
                    { 47, 2, false, "Room 47", "47", "Standard", true },
                    { 48, 2, false, "Room 48", "48", "Standard", true },
                    { 49, 2, false, "Room 49", "49", "Standard", true },
                    { 50, 2, false, "Room 50", "50", "Standard", true },
                    { 51, 2, false, "Room 51", "51", "Standard", true },
                    { 52, 2, false, "Room 52", "52", "Standard", true },
                    { 53, 2, false, "Room 53", "53", "Standard", true },
                    { 54, 2, false, "Room 54", "54", "Standard", true },
                    { 55, 2, false, "Room 55", "55", "Standard", true },
                    { 56, 2, false, "Room 56", "56", "Standard", true },
                    { 57, 2, false, "Room 57", "57", "Standard", true },
                    { 58, 2, false, "Room 58", "58", "Standard", true },
                    { 59, 2, false, "Room 59", "59", "Standard", true },
                    { 60, 2, false, "Room 60", "60", "Standard", true },
                    { 61, 2, false, "Room 61", "61", "Standard", true },
                    { 62, 2, false, "Room 62", "62", "Standard", true },
                    { 63, 2, false, "Room 63", "63", "Standard", true },
                    { 64, 2, false, "Room 64", "64", "Standard", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 64);
        }
    }
}
