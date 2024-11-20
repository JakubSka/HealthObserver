using HealthObserver.BlobStorage;
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
        private readonly BlobStorageService _blobService;
        public DisplayModel(ApplicationDbContext dbContext, BlobStorageService blobService)
        {
            _dbContext = dbContext;
            _blobService = blobService;
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
        public async Task<IActionResult> OnPostDownloadAsync(int surveyId)
        {
            var survey = await _dbContext.PatientSurveys.FindAsync(surveyId);
            if (survey == null || string.IsNullOrEmpty(survey.FileName))
            {
                return NotFound();
            }

            var fileStream = await _blobService.GetFileAsync(survey.FileName);
            if (fileStream == null)
            {
                return NotFound();
            }

            return File(fileStream, "application/pdf", survey.FileName);
        }
        public IActionResult OnGetPreview(int surveyId)
        {
            var survey = _dbContext.PatientSurveys.FirstOrDefault(s => s.PatientSurveyId == surveyId);
            if (survey == null || string.IsNullOrEmpty(survey.FileName))
            {
                return NotFound();
            }

            var fileUrl = _blobService.GetFileUrl(survey.FileName);
            if (string.IsNullOrEmpty(fileUrl))
            {
                return NotFound();
            }

            return Redirect(fileUrl); 
        }
    }
}
