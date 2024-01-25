using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class startAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Apartments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBathrooms",
                table: "Apartments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRooms",
                table: "Apartments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RentPerMonth",
                table: "Apartments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "NumberOfBathrooms",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "NumberOfRooms",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "RentPerMonth",
                table: "Apartments");
        }
    }
}
