using HealthObserver.Data;
using HealthObserver.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HealthObserver.Pages.Patient
{
	public class MySurveyModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;

		public MySurveyModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}


		[BindProperty]
		public PatientSurvey PatientSurvey { get; set; }

		[BindProperty]
		public DateTime ResponseDate { get; set; }

        [BindProperty]
        public int SurveyHour { get; set; }

        public async Task<IActionResult> OnGetAsync()
		{




			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{

            ApplicationUser user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                
                return RedirectToPage("/Error");
            }

            PatientSurvey.UserId = user.Id;
            DateTime surveyDateTime = new DateTime(PatientSurvey.ResponseDate.Year, PatientSurvey.ResponseDate.Month, PatientSurvey.ResponseDate.Day, 0, 0, 0);

            

	        if (_context.PatientSurveys.Any(ps =>
		        ps.UserId == user.Id && ps.ResponseDate == surveyDateTime &&
				ps.SurveyHour == PatientSurvey.SurveyHour))
	        {
		        TempData["ShowAlert"] = true;
		        TempData["AlertType"] = "danger";
		        TempData["AlertMessage"] = "A survey with the same date and time already exists.";

		        return Page();
	        }
			else if(surveyDateTime > DateTime.Now.Date)
			{
				TempData["ShowAlert"] = true;
				TempData["AlertType"] = "danger";
				TempData["AlertMessage"] = "You cannot complete survey for the future.";

				return Page();
			}
            

            _context.PatientSurveys.Add(PatientSurvey);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Patient/MySurvey");
        }

			
		
	}

}
