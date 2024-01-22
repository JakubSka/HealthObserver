using HealthObserver.Data;
using HealthObserver.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HealthObserver.Pages.Doctor
{
	public class MedicineListModel : PageModel
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public MedicineListModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public List<Medicine> Medicines { get; set; }

		[BindProperty]
		public Medicine Medicine { get; set; }

		

		public async Task<IActionResult> OnGetAsync()
		{
			Medicines = await _context.Medicines.ToListAsync();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid || string.IsNullOrWhiteSpace(Medicine.Name) || string.IsNullOrWhiteSpace(Medicine.Description))
			{
				ModelState.AddModelError(string.Empty, "Name and Description are required fields.");
				return Page();
			}

			var currentUser = await _userManager.GetUserAsync(User);
			var userRoles = await _userManager.GetRolesAsync(currentUser);

			if (userRoles.Contains("Doctor"))
			{
				_context.Medicines.Add(Medicine);
				await _context.SaveChangesAsync();
				return RedirectToPage("MedicineList");
			}

			return Forbid();
		}

		public async Task<IActionResult> OnPostEditAsync(int medicineId)
		{
			if (!ModelState.IsValid || string.IsNullOrWhiteSpace(Medicine.Name) || string.IsNullOrWhiteSpace(Medicine.Description))
			{
				ModelState.AddModelError(string.Empty, "Name and Description are required fields.");
				return Page();
			}

			var currentUser = await _userManager.GetUserAsync(User);
			var userRoles = await _userManager.GetRolesAsync(currentUser);

			if (userRoles.Contains("Doctor"))
			{
				var medicineToUpdate = await _context.Medicines.FindAsync(medicineId);

				if (medicineToUpdate != null)
				{
					medicineToUpdate.Name = Medicine.Name;
					medicineToUpdate.Description = Medicine.Description;

					await _context.SaveChangesAsync();
				}

				return RedirectToPage("MedicineList");
			}

			return Forbid();
		}
	}
}

