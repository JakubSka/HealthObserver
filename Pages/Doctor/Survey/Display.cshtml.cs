using HealthObserver.Data;
using HealthObserver.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HealthObserver.Pages.Doctor.Survey
{
    public class DisplayModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public DisplayModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }

        public List<Models.PatientSurvey> PatientSurveys { get; set; }

		public ApplicationUser user { get; set; }

		public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return NotFound();
            }

	        user = await _dbContext.ApplicationUsers.FindAsync(UserId);
            if (user == null)
            {
                return NotFound();
            }

            IQueryable<Models.PatientSurvey> surveysQuery = _dbContext.PatientSurveys
                .Where(p => p.UserId == UserId);

           
            switch (SortOrder)
            {
                case "oldest":
                    surveysQuery = surveysQuery.OrderBy(s => s.ResponseDate);
                    break;
                case "newest":
                    surveysQuery = surveysQuery.OrderByDescending(s => s.ResponseDate);
                    break;
                default:
                    surveysQuery = surveysQuery.OrderBy(s => s.ResponseDate);
                    break;
            }

            PatientSurveys = await surveysQuery.ToListAsync();

            return Page();
        }
    }
}
