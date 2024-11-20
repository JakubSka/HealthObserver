using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthObserver.Data.Migrations
{
    /// <inheritdoc />
    public partial class filenametosurvey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "PatientSurveys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "PatientSurveys");
        }
    }
}
