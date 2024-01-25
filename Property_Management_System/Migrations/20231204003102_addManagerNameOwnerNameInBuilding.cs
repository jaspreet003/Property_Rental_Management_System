using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class addManagerNameOwnerNameInBuilding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Buildings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ManagerName",
                table: "Buildings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "Buildings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "ManagerName",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "Buildings");
        }
    }
}
