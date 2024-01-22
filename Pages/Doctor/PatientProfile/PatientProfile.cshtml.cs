using HealthObserver.Data;
using HealthObserver.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HealthObserver.Pages.Doctor.PatientProfile
{
    public class PatientProfileModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public PatientProfileModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }

        public List<Models.Prescription> Prescriptions { get; set; }

        public List<Medicine> Medicines { get; set; }

        [BindProperty] 
        public int SelectedMedicineId { get; set; }

        [BindProperty] 
        public double Dose { get; set; }

        [BindProperty] 
        public string DoseUnit { get; set; }

        [BindProperty] 
        public int FrequencyInHours { get; set; }

        [BindProperty]
        public DateTime StartPrescription { get; set; }

        [BindProperty] 
        public DateTime EndPrescription { get; set; }

        public string ErrorMessage { get; set; }

		public ApplicationUser user { get; set; }

		public async Task<IActionResult> OnGetAsync()
        {
	        ErrorMessage = TempData["ErrorMessage"] as string;

			if (string.IsNullOrEmpty(UserId))
			{
				return NotFound();
			}

			user = await _dbContext.ApplicationUsers.FindAsync(UserId);

			if (user == null)
			{
				return NotFound();
			}

			Prescriptions = await _dbContext.Prescriptions
				.Include(p => p.Medicine)
				.Where(p => p.UserId == UserId && p.EndPrescription.Date >= DateTime.Now.Date)
                .ToListAsync();
			Medicines = await _dbContext.Medicines.ToListAsync();


			return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {

	        if (SelectedMedicineId > 0)
	        {
		        if (EndPrescription < StartPrescription)
		        {
			        TempData["ErrorMessage"] = "End date cannot be earlier than start date.";

					user = await _dbContext.ApplicationUsers.FindAsync(UserId);

					return RedirectToPage("/Doctor/PatientProfile/PatientProfile", new { userId = UserId });
				}
		        if(Dose <0)
		        {
					TempData["ErrorMessage"] = "dose.";

					user = await _dbContext.ApplicationUsers.FindAsync(UserId);

					return RedirectToPage("/Doctor/PatientProfile/PatientProfile", new { userId = UserId });
				}
		        if (DoseUnit == null)
		        {
			        TempData["ErrorMessage"] = "DoseUnit cannot be null";

			        user = await _dbContext.ApplicationUsers.FindAsync(UserId);

			        return RedirectToPage("/Doctor/PatientProfile/PatientProfile", new { userId = UserId });
		        }
		        if (FrequencyInHours <0)
		        {
			        TempData["ErrorMessage"] = "Freq.";

			        user = await _dbContext.ApplicationUsers.FindAsync(UserId);

			        return RedirectToPage("/Doctor/PatientProfile/PatientProfile", new { userId = UserId });
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

	        return RedirectToPage("/Doctor/PatientProfile/PatientProfile", new { userId = UserId });
        }


		public async Task<IActionResult> OnPostEditAsync(int prescriptionId)
		{

			UserId = Request.Form["UserId"];

			if (!ModelState.IsValid)
			{
				return Page();
			}

			var prescriptionToUpdate = await _dbContext.Prescriptions.FindAsync(prescriptionId);

			if (prescriptionToUpdate != null)
			{
				if (EndPrescription < StartPrescription)
				{
					user = await _dbContext.ApplicationUsers.FindAsync(UserId);
					TempData["ErrorMessage"] = "End date cannot be earlier than start date.";
					return RedirectToPage("/Doctor/PatientProfile/PatientProfile", new { userId = UserId });
				}
				if (Dose < 0)
				{
					

					user = await _dbContext.ApplicationUsers.FindAsync(UserId);
					TempData["ErrorMessage"] = "dose.";
					return RedirectToPage("/Doctor/PatientProfile/PatientProfile", new { userId = UserId });
				}
				if (DoseUnit == null)
				{
					

					user = await _dbContext.ApplicationUsers.FindAsync(UserId);
					TempData["ErrorMessage"] = "DoseUnit cannot be null";
					return RedirectToPage("/Doctor/PatientProfile/PatientProfile", new { userId = UserId });
				}
				if (FrequencyInHours < 0)
				{
					

					user = await _dbContext.ApplicationUsers.FindAsync(UserId);
					TempData["ErrorMessage"] = "Freq.";
					return RedirectToPage("/Doctor/PatientProfile/PatientProfile", new { userId = UserId });
				}
				prescriptionToUpdate.Dose = Dose;
				prescriptionToUpdate.DoseUnit = DoseUnit;
				prescriptionToUpdate.FrequencyInHours = FrequencyInHours;
				prescriptionToUpdate.StartPrescription = StartPrescription;
				prescriptionToUpdate.EndPrescription = EndPrescription;
				
				await _dbContext.SaveChangesAsync();

			}

			user = await _dbContext.ApplicationUsers.FindAsync(UserId);

			return RedirectToPage("/Doctor/PatientProfile/PatientProfile", new { userId = UserId });
		}
	}
}
