using HealthObserver.Data;
using HealthObserver.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HealthObserver.Pages.Doctor.Prescription
{
    public class HistoryPrescriptionModel : PageModel
    {

	    private readonly ApplicationDbContext _dbContext;

	    public HistoryPrescriptionModel(ApplicationDbContext dbContext)
	    {
			_dbContext=dbContext;
	    }

		public List<Medicine> Medicines { get; set; }

		

		[BindProperty(SupportsGet = true)] public string UserId { get; set; }

		[BindProperty] public double Dose { get; set; }

		[BindProperty] public string DoseUnit { get; set; }

		[BindProperty] public int FrequencyInHours { get; set; }

		[BindProperty] public DateTime StartPrescription { get; set; }

		[BindProperty] public DateTime EndPrescription { get; set; }

		[BindProperty]
		public int PrescriptionId { get; set; }


		public string UserName { get; set; }

		[BindProperty(SupportsGet = true)]
		public string MedicationHistorySortOrder { get; set; }

		public List<Models.Prescription> UserPrescriptions { get; set; }
		public List<MedicationHistory> MedicationHistory { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			if (string.IsNullOrEmpty(UserId))
			{
				return NotFound();
			}

			var user = await _dbContext.Users.FindAsync(UserId);
			if (user == null)
			{
				return NotFound();
			}



			Medicines = await _dbContext.Medicines.ToListAsync();
			UserPrescriptions = await _dbContext.Prescriptions
				.Include(p => p.Medicine)
				.Where(p => p.UserId == UserId && p.EndPrescription.Date < DateTime.Now.Date)
				.OrderBy(p => MedicationHistorySortOrder == "oldest" ? p.EndPrescription : DateTime.MinValue)
				.ThenByDescending(p => MedicationHistorySortOrder == "newest" ? p.EndPrescription : DateTime.MinValue)
				.ToListAsync();

			MedicationHistory = await _dbContext.MedicationHistory
				.Include(h => h.Prescription.Medicine)
				.Where(h => h.Prescription.UserId == UserId)
				.OrderBy(h => MedicationHistorySortOrder == "oldest" ? h.TakenDateTime : DateTime.MinValue)
				.ThenByDescending(h => MedicationHistorySortOrder == "newest" ? h.TakenDateTime : DateTime.MinValue)
				.ToListAsync();


			return Page();
		}
	}
}
