using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class changesForBuildingsName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Buildings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Buildings");
        }
    }
}
