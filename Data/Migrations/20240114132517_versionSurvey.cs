using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthObserver.Data.Migrations
{
    /// <inheritdoc />
    public partial class versionSurvey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SurveyDate",
                table: "PatientSurveys",
                newName: "SurveyDateTime");

            migrationBuilder.AddColumn<int>(
                name: "PatientSurveyId",
                table: "SurveyQuestions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SurveyHour",
                table: "PatientSurveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResponseDate",
                table: "PatientSurveyResponses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestions_PatientSurveyId",
                table: "SurveyQuestions",
                column: "PatientSurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestions_PatientSurveys_PatientSurveyId",
                table: "SurveyQuestions",
                column: "PatientSurveyId",
                principalTable: "PatientSurveys",
                principalColumn: "PatientSurveyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestions_PatientSurveys_PatientSurveyId",
                table: "SurveyQuestions");

            migrationBuilder.DropIndex(
                name: "IX_SurveyQuestions_PatientSurveyId",
                table: "SurveyQuestions");

            migrationBuilder.DropColumn(
                name: "PatientSurveyId",
                table: "SurveyQuestions");

            migrationBuilder.DropColumn(
                name: "SurveyHour",
                table: "PatientSurveys");

            migrationBuilder.DropColumn(
                name: "ResponseDate",
                table: "PatientSurveyResponses");

            migrationBuilder.RenameColumn(
                name: "SurveyDateTime",
                table: "PatientSurveys",
                newName: "SurveyDate");
        }
    }
}
