using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthObserver.Data.Migrations
{
    /// <inheritdoc />
    public partial class newsurvey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientSurveyResponses");

            migrationBuilder.DropTable(
                name: "SurveyQuestions");

            migrationBuilder.RenameColumn(
                name: "SurveyDateTime",
                table: "PatientSurveys",
                newName: "ResponseDate");

            migrationBuilder.AddColumn<int>(
                name: "AbdominalPainRating",
                table: "PatientSurveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalComments",
                table: "PatientSurveys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnergyRating",
                table: "PatientSurveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FatigueRating",
                table: "PatientSurveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GeneralWellBeingRating",
                table: "PatientSurveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HeadacheRating",
                table: "PatientSurveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NauseaRating",
                table: "PatientSurveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StressRating",
                table: "PatientSurveys",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AbdominalPainRating",
                table: "PatientSurveys");

            migrationBuilder.DropColumn(
                name: "AdditionalComments",
                table: "PatientSurveys");

            migrationBuilder.DropColumn(
                name: "EnergyRating",
                table: "PatientSurveys");

            migrationBuilder.DropColumn(
                name: "FatigueRating",
                table: "PatientSurveys");

            migrationBuilder.DropColumn(
                name: "GeneralWellBeingRating",
                table: "PatientSurveys");

            migrationBuilder.DropColumn(
                name: "HeadacheRating",
                table: "PatientSurveys");

            migrationBuilder.DropColumn(
                name: "NauseaRating",
                table: "PatientSurveys");

            migrationBuilder.DropColumn(
                name: "StressRating",
                table: "PatientSurveys");

            migrationBuilder.RenameColumn(
                name: "ResponseDate",
                table: "PatientSurveys",
                newName: "SurveyDateTime");

            migrationBuilder.CreateTable(
                name: "SurveyQuestions",
                columns: table => new
                {
                    SurveyQuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientSurveyId = table.Column<int>(type: "int", nullable: true),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestions", x => x.SurveyQuestionId);
                    table.ForeignKey(
                        name: "FK_SurveyQuestions_PatientSurveys_PatientSurveyId",
                        column: x => x.PatientSurveyId,
                        principalTable: "PatientSurveys",
                        principalColumn: "PatientSurveyId");
                });

            migrationBuilder.CreateTable(
                name: "PatientSurveyResponses",
                columns: table => new
                {
                    SurveyQuestionResponseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientSurveyId = table.Column<int>(type: "int", nullable: false),
                    SurveyQuestionId = table.Column<int>(type: "int", nullable: false),
                    Response = table.Column<int>(type: "int", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientSurveyResponses", x => x.SurveyQuestionResponseId);
                    table.ForeignKey(
                        name: "FK_PatientSurveyResponses_PatientSurveys_PatientSurveyId",
                        column: x => x.PatientSurveyId,
                        principalTable: "PatientSurveys",
                        principalColumn: "PatientSurveyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientSurveyResponses_SurveyQuestions_SurveyQuestionId",
                        column: x => x.SurveyQuestionId,
                        principalTable: "SurveyQuestions",
                        principalColumn: "SurveyQuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientSurveyResponses_PatientSurveyId",
                table: "PatientSurveyResponses",
                column: "PatientSurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSurveyResponses_SurveyQuestionId",
                table: "PatientSurveyResponses",
                column: "SurveyQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestions_PatientSurveyId",
                table: "SurveyQuestions",
                column: "PatientSurveyId");
        }
    }
}
