using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthObserver.Models
{
	public class Prescription
	{
		[Key]
		public int PrescriptionId { get; set; }

		[ForeignKey("ApplicationUser")]
		public string UserId { get; set; }

		[ForeignKey("Medicine")]
		public int MedicineId { get; set; }

		public ApplicationUser ApplicationUser { get; set; }
		public Medicine Medicine { get; set; }

		public double Dose { get; set; }
		public string DoseUnit { get; set; }
		public int FrequencyInHours { get; set; }
		
		public double AmountTaken { get; set; }

		public DateTime StartPrescription { get; set; }

		public DateTime EndPrescription { get; set; }

		public List<MedicationHistory> MedicationHistory { get; set; }

	}
}
