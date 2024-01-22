using HealthObserver.Data;
using HealthObserver.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HealthObserver.Pages.Doctor.Prescription
{
	public class AddModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public AddModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public List<Medicine> Medicines { get; set; }

		[BindProperty] public int SelectedMedicineId { get; set; }

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
				.Where(p => p.UserId == UserId && p.EndPrescription.Date >= DateTime.Now.Date)
				.ToListAsync();

            MedicationHistory = await _dbContext.MedicationHistory
                .Include(h => h.Prescription.Medicine)
                .Where(h => h.Prescription.UserId == UserId)
                .OrderBy(h => MedicationHistorySortOrder == "oldest" ? h.TakenDateTime : DateTime.MinValue)
                .ThenByDescending(h => MedicationHistorySortOrder == "newest" ? h.TakenDateTime : DateTime.MinValue)
                .ToListAsync();


            return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{

			if (SelectedMedicineId > 0)
			{
				if (EndPrescription < StartPrescription)
				{
					ModelState.AddModelError("EndPrescription", "End date cannot be earlier than start date.");
					return Page();
				}


				var prescription = new Models.Prescription
				{
					UserId = UserId,
					MedicineId = SelectedMedicineId,
					Dose = Dose,
					DoseUnit = DoseUnit,
					FrequencyInHours = FrequencyInHours,
					StartPrescription = StartPrescription,
					EndPrescription = EndPrescription
				};

				_dbContext.Prescriptions.Add(prescription);
				await _dbContext.SaveChangesAsync();
			}


			UserPrescriptions = await _dbContext.Prescriptions
				.Include(p => p.Medicine)
				.Where(p => p.UserId == UserId )
				.ToListAsync();

			if (!string.IsNullOrEmpty(Request.Query["handler"]) && Request.Query["handler"] == "Edit")
			{
				if (int.TryParse(Request.Query["Id"], out int prescriptionId))
				{
					var prescriptionToUpdate = await _dbContext.Prescriptions.FindAsync(prescriptionId);

					if (prescriptionToUpdate != null)
					{

						prescriptionToUpdate.Dose = Dose;
						prescriptionToUpdate.DoseUnit = DoseUnit;
						prescriptionToUpdate.FrequencyInHours = FrequencyInHours;
						prescriptionToUpdate.StartPrescription = StartPrescription;
						prescriptionToUpdate.EndPrescription= EndPrescription;
						await _dbContext.SaveChangesAsync();
					}
				}

			}

			return RedirectToPage("/Doctor/Prescription/Add", new { userId = UserId });
		}
	}
}