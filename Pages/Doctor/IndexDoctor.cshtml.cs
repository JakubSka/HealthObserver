using HealthObserver.Data;
using HealthObserver.Data.Migrations;
using HealthObserver.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HealthObserver.Pages.Doctor
{
    public class IndexDoctorModel : PageModel
    {
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _dbContext;

		public IndexDoctorModel(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
		{
			this._roleManager = roleManager;
			this._userManager = userManager;
			this._dbContext = dbContext;

		}
		public List<IdentityRole> Roles { get; set; }
		public List<ApplicationUser> UsersWithRolePatient { get; set; }
		public List<Medicine> Medicines { get; set; }

		[BindProperty]
		public int SelectedMedicineId { get; set; }

		[BindProperty(SupportsGet = true)]
		public string SearchQuery { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			Roles = await _roleManager.Roles.ToListAsync();
			UsersWithRolePatient = new List<ApplicationUser>();
			Medicines= await _dbContext.Medicines.ToListAsync();

			var allUsers = await _userManager.Users.ToListAsync();
			UsersWithRolePatient = string.IsNullOrEmpty(SearchQuery)
				? allUsers.Where(u => _userManager.GetRolesAsync(u).Result.Contains("Patient")).ToList()
				: allUsers.Where(u =>
					_userManager.GetRolesAsync(u).Result.Contains("Patient") &&
					(u.FirstName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
					 u.LastName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))).ToList();

			return Page();
		}
		public async Task<IActionResult> OnPostAddPrescriptionAsync(string userId)
		{
			if (!string.IsNullOrEmpty(userId) && SelectedMedicineId > 0)
			{
				var prescription = new Models.Prescription
				{
					UserId = userId,
					MedicineId = SelectedMedicineId
				};

				_dbContext.Prescriptions.Add(prescription);
				await _dbContext.SaveChangesAsync();
			}

			return RedirectToPage();
		}

	}
}