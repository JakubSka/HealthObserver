using HealthObserver.Data;
using HealthObserver.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthObserver.Pages.Doctor.Diagrams
{
    public class DisplayDiagramModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PatientSurvey Survey { get; set; }
        public List<string> ChartLabels { get; set; }
        public List<int> GeneralWellBeingRatingData { get; set; }
        public List<int> HeadacheRatingData { get; set; }
        public List<int> AbdominalPainData { get; set; }
        public List<int> NauseaRatingData { get; set; }
        public List<int> FatigueRatingData { get; set; }
        public List<int> StressRatingData { get; set; }
        public List<int> EnergyRatingData { get; set; }

        [BindProperty(SupportsGet = true)] public string UserId { get; set; }

        public ApplicationUser user { get; set; }

		public DisplayDiagramModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int surveyId)
        {
            Survey = await _context.PatientSurveys.FindAsync(surveyId);

            user = await _context.ApplicationUsers.FindAsync(UserId);

			if (Survey == null)
            {
                return NotFound();
            }

            

            var surveysForDay = _context.PatientSurveys
                .Where(s => s.ResponseDate.Date == Survey.ResponseDate.Date && s.UserId==UserId)
                .OrderBy(s => s.SurveyHour)
                .ToList();

            
            ChartLabels = surveysForDay.Select(s => s.SurveyHour.ToString()).ToList();
            GeneralWellBeingRatingData = surveysForDay.Select(s => s.GeneralWellBeingRating).ToList();
            HeadacheRatingData = surveysForDay.Select(s => s.HeadacheRating).ToList();
            AbdominalPainData = surveysForDay.Select(s => s.AbdominalPainRating).ToList();
            NauseaRatingData = surveysForDay.Select(s => s.NauseaRating).ToList();
            FatigueRatingData = surveysForDay.Select(s => s.FatigueRating).ToList();
            StressRatingData = surveysForDay.Select(s => s.StressRating).ToList();
            EnergyRatingData = surveysForDay.Select(s => s.EnergyRating).ToList();


            return Page();
        }

        private int GetRatingForHour(int hour)
        {
            
            return Survey.SurveyHour == hour ? Survey.GeneralWellBeingRating : 0;
        }
    }
}