using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthObserver.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSurveycorr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientSurveys_SurveyQuestions_SurveyQuestionId",
                table: "PatientSurveys");

            migrationBuilder.DropIndex(
                name: "IX_PatientSurveys_SurveyQuestionId",
                table: "PatientSurveys");

            migrationBuilder.DropColumn(
                name: "Response",
                table: "PatientSurveys");

            migrationBuilder.DropColumn(
                name: "SurveyQuestionId",
                table: "PatientSurveys");

            migrationBuilder.CreateTable(
                name: "PatientSurveyResponses",
                columns: table => new
                {
                    SurveyQuestionResponseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientSurveyId = table.Column<int>(type: "int", nullable: false),
                    SurveyQuestionId = table.Column<int>(type: "int", nullable: false),
                    Response = table.Column<int>(type: "int", nullable: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientSurveyResponses");

            migrationBuilder.AddColumn<int>(
                name: "Response",
                table: "PatientSurveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SurveyQuestionId",
                table: "PatientSurveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PatientSurveys_SurveyQuestionId",
                table: "PatientSurveys",
                column: "SurveyQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientSurveys_SurveyQuestions_SurveyQuestionId",
                table: "PatientSurveys",
                column: "SurveyQuestionId",
                principalTable: "SurveyQuestions",
                principalColumn: "SurveyQuestionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
