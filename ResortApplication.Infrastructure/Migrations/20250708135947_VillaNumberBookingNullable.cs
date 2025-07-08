using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResortApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VillaNumberBookingNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_VillaNumbers_VillaNumberId",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "VillaNumberId",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_VillaNumbers_VillaNumberId",
                table: "Bookings",
                column: "VillaNumberId",
                principalTable: "VillaNumbers",
                principalColumn: "VillaNumberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_VillaNumbers_VillaNumberId",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "VillaNumberId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_VillaNumbers_VillaNumberId",
                table: "Bookings",
                column: "VillaNumberId",
                principalTable: "VillaNumbers",
                principalColumn: "VillaNumberId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
