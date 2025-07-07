using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResortApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAmenityDescriptionField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Amenities",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Amenities");
        }
    }
}
