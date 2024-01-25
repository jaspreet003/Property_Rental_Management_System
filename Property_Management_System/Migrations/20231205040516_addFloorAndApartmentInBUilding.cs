using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class addFloorAndApartmentInBUilding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NbApartment",
                table: "Buildings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NbFloor",
                table: "Buildings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NbApartment",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "NbFloor",
                table: "Buildings");
        }
    }
}
