using HealthObserver.Data;
using HealthObserver.Data.Migrations;
using HealthObserver.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HealthObserver.Pages.Patient
{
    public class DashboardPatientModel : PageModel
    {
	    private readonly UserManager<ApplicationUser> _userManager;
	    private readonly ApplicationDbContext _dbContext;

	    public DashboardPatientModel(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
	    {
		    _userManager = userManager;
		    _dbContext = dbContext;
	    }

	    public List<MedicationHistory> MedicationHistory { get; set; }
	    
	    public List<Prescription> Prescriptions { get; set; }

		public int TodaySurveyCount { get; set; }

		public int TodayMedicationCount { get; set; }
		public int TodayMedicationAllCount { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			var currentUser = await _userManager.GetUserAsync(User);

			MedicationHistory = await _dbContext.MedicationHistory
				.Include(h => h.Prescription)
				.Where(h => h.Prescription.UserId == currentUser.Id && h.TakenDateTime.Date== DateTime.Now.Date && h.AmountTaken==h.Prescription.Dose)
				.ToListAsync();

			Prescriptions = await _dbContext.Prescriptions
				.Include(p => p.Medicine)
				.Where(p => p.UserId == currentUser.Id && p.EndPrescription.Date >= DateTime.Now.Date && p.StartPrescription.Date<= DateTime.Now.Date)
				.ToListAsync();

			var todaySurveys = await _dbContext.PatientSurveys
				.Where(s => s.UserId == currentUser.Id &&
				            s.ResponseDate.Date == DateTime.Now.Date)
				.ToListAsync();

			TodayMedicationCount = MedicationHistory.Count;
			TodaySurveyCount = todaySurveys.Count;

			TodayMedicationAllCount = Prescriptions.Count;
			
			return Page();
		}
	}
}
