using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class addVisitControllercs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Visits",
                newName: "Slot");

            migrationBuilder.AddColumn<int>(
                name: "ApartmentId",
                table: "Visits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Visits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Visits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_BuildingId",
                table: "Visits",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Buildings_BuildingId",
                table: "Visits",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Buildings_BuildingId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_BuildingId",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "ApartmentId",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Visits");

            migrationBuilder.RenameColumn(
                name: "Slot",
                table: "Visits",
                newName: "UserId");
        }
    }
}
