using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthObserver.Data.Migrations
{
    /// <inheritdoc />
    public partial class dose : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Dose",
                table: "Prescriptions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DoseUnit",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FrequencyInHours",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dose",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "DoseUnit",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "FrequencyInHours",
                table: "Prescriptions");
        }
    }
}
