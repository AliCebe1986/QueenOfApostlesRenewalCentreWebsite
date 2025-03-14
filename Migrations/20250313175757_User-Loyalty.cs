using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueenOfApostlesRenewalCentre.Migrations
{
    /// <inheritdoc />
    public partial class UserLoyalty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "loyalty",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "loyalty",
                table: "AspNetUsers");
        }
    }
}
