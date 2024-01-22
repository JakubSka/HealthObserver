using HealthObserver.Data;
using HealthObserver.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

namespace HealthObserver.Pages.Doctor.Diagrams
{
    public class DiagramSurveyModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<PatientSurvey> Surveys { get; set; }

        public DiagramSurveyModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)] public string UserId { get; set; }

        [BindProperty]
        public DateTime ResponseDate { get; set; }

        public ApplicationUser user { get; set; }

		[BindProperty(SupportsGet = true)]
        public string DiagramSortOrder { get; set; }

		public async Task<IActionResult> OnGetAsync()
        {
			IQueryable<PatientSurvey> surveysQuery = _context.PatientSurveys;

			user = await _context.ApplicationUsers.FindAsync(UserId);

			if (DiagramSortOrder == "oldest")
			{
				Surveys = await surveysQuery.OrderBy(s => s.ResponseDate).ToListAsync();
			}
			else if (DiagramSortOrder == "newest")
			{
				Surveys = await surveysQuery.OrderByDescending(s => s.ResponseDate).ToListAsync();
			}
			else
			{
				Surveys = await surveysQuery.ToListAsync();
			}

			
			Surveys = Surveys.GroupBy(s => s.ResponseDate).Select(group => group.First()).ToList();


			return Page();
        }
    }
}