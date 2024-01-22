using HealthObserver.Data;
using HealthObserver.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HealthObserver.Pages.Patient
{
	public class MyPrescriptionsModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _dbContext;

		public MyPrescriptionsModel(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
		{
			_userManager = userManager;
			_dbContext = dbContext;
		}

		public List<Prescription> Prescriptions { get; set; }
		public List<MedicationHistory> MedicationHistory { get; set; }

		[BindProperty]
		public int SelectedPrescriptionId { get; set; }

		[BindProperty]
		public int MedicationHistoryId { get; set; }

		[BindProperty]
		public double AmountTaken { get; set; }

		[BindProperty]
		[DataType(DataType.Date)]
		public DateTime TakenDateTime { get; set; }


		[BindProperty(SupportsGet = true)]
		[DataType(DataType.Date)]
		public DateTime DateSearchQuery { get; set; }

		[BindProperty(SupportsGet = true)]
		public string MedicationHistorySortOrder { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			var currentUser = await _userManager.GetUserAsync(User);

			IQueryable<MedicationHistory> medicationHistoryQuery = _dbContext.MedicationHistory;

			if (DateSearchQuery != default)
			{
				medicationHistoryQuery = medicationHistoryQuery
					.Where(m => m.TakenDateTime.Date == DateSearchQuery.Date);
			}
			MedicationHistory = await medicationHistoryQuery.Include(h => h.Prescription.Medicine)
				.Where(h => h.Prescription.UserId == currentUser.Id)
				.OrderBy(p => MedicationHistorySortOrder == "oldest" ? p.TakenDateTime : DateTime.MinValue)
				.ThenByDescending(p => MedicationHistorySortOrder == "newest" ? p.TakenDateTime : DateTime.MinValue).ToListAsync();

			Prescriptions = await _dbContext.Prescriptions
				.Include(p => p.Medicine)
				.Where(p => p.UserId == currentUser.Id && p.EndPrescription.Date >= DateTime.Now.Date && p.StartPrescription<= DateTime.Now.Date)
				.ToListAsync();


			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var currentUser = await _userManager.GetUserAsync(User);

			var prescript = await _dbContext.Prescriptions
				.Where(h => h.UserId == currentUser.Id && h.PrescriptionId == SelectedPrescriptionId)
				.FirstOrDefaultAsync();

			var existingEntry = await _dbContext.MedicationHistory
				.Where(h => h.Prescription.UserId == currentUser.Id
                            && h.TakenDateTime == TakenDateTime
							&& h.PrescriptionId== SelectedPrescriptionId)
				.FirstOrDefaultAsync();

			if (existingEntry != null)
			{
				TempData["AlertType"] = "danger";
				TempData["AlertMessage"] = "Entry with this date already exists.";
				TempData["ShowAlert"] = true;
			}
			else
			{
				if (SelectedPrescriptionId > 0)
				{
					if (AmountTaken < 0)
					{
						TempData["AlertType"] = "danger";
						TempData["AlertMessage"] = "AmountTaken must be a non-negative value.";
						TempData["ShowAlert"] = true;
					}else if(TakenDateTime>DateTime.Now.Date || TakenDateTime < prescript.StartPrescription)
					{
						TempData["AlertType"] = "danger";
						TempData["AlertMessage"] = "Taken date time cant be";
						TempData["ShowAlert"] = true;
					}
					else
					{
						var history = new Models.MedicationHistory
						{
							AmountTaken = AmountTaken,
							MedicationHistoryId = MedicationHistoryId,
							PrescriptionId = SelectedPrescriptionId,
							TakenDateTime = TakenDateTime
						};


						_dbContext.MedicationHistory.Add(history);
						await _dbContext.SaveChangesAsync();
					}
				}
			}

			MedicationHistory = await _dbContext.MedicationHistory
				.Include(h => h.Prescription)
				.Where(h => h.Prescription.UserId == currentUser.Id)
				.OrderByDescending(h => h.TakenDateTime)
				.ToListAsync();

			if (!string.IsNullOrEmpty(Request.Query["handler"]) && Request.Query["handler"] == "Edit")
			{
				if (int.TryParse(Request.Query["Id"], out int medicationHistoryId))
				{
					var historyToUpdate = await _dbContext.MedicationHistory.FindAsync(medicationHistoryId);

					if (historyToUpdate != null)
					{
						if (AmountTaken < 0)
						{
							TempData["AlertType"] = "danger";
							TempData["AlertMessage"] = "AmountTaken must be a non-negative value.";
							TempData["ShowAlert"] = true;
							AmountTaken = 0;

						}
						historyToUpdate.AmountTaken = AmountTaken;
						await _dbContext.SaveChangesAsync();
					}
				}

			}

			return RedirectToPage();
		}


	}
}