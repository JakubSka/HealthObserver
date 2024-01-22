using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthObserver.Models
{
	public class MedicationHistory
	{
		[Key]
		public int MedicationHistoryId { get; set; }

		[ForeignKey("Prescription")]
		public int PrescriptionId { get; set; }

		public Prescription Prescription { get; set; }

		public double AmountTaken { get; set; }
		public DateTime TakenDateTime { get; set; }
	}
}
