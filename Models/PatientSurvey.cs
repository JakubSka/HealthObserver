using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthObserver.Models
{
	public class PatientSurvey
	{
		[Key]
		public int PatientSurveyId { get; set; }

		[ForeignKey("ApplicationUser")]
		public string UserId { get; set; }

		public ApplicationUser ApplicationUser { get; set; }

		public int SurveyHour { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime ResponseDate { get; set; }

		[Range(1, 10, ErrorMessage = "The rating must be between 1 and 10.")]
		public int GeneralWellBeingRating { get; set; } // Ocena ogólnego samopoczucia
		[Range(1, 10, ErrorMessage = "The rating must be between 1 and 10.")]
		public int HeadacheRating { get; set; } // Ocena poziomu bólu głowy
		[Range(1, 10, ErrorMessage = "The rating must be between 1 and 10.")]
		public int AbdominalPainRating { get; set; } // Ocena poziomu bólu brzucha
		[Range(1, 10, ErrorMessage = "The rating must be between 1 and 10.")]
		public int NauseaRating { get; set; } // Ocena poziomu nudności
		[Range(1, 10, ErrorMessage = "The rating must be between 1 and 10.")]
		public int FatigueRating { get; set; } // Ocena poziomu zmęczenia
		[Range(1, 10, ErrorMessage = "The rating must be between 1 and 10.")]
		public int StressRating { get; set; } // Ocena poziomu stresu
		[Range(1, 10, ErrorMessage = "The rating must be between 1 and 10.")]
		public int EnergyRating { get; set; } // Ocena poziomu energii
		public string? AdditionalComments { get; set; }
        public string FileName { get; set; }
    }
}
