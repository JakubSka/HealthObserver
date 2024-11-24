using HealthObserver.Data;
using HealthObserver.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using HealthObserver.BlobStorage;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
namespace HealthObserver.Pages.Patient
{
	public class MySurveyModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;
        private readonly BlobStorageService _blobStorageService;

		public MySurveyModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context, BlobStorageService blobStorageService)
		{
			_userManager = userManager;
			_context = context;
            _blobStorageService = blobStorageService;
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
        public async Task<string> GenerateSurveyPdfAsync(PatientSurvey survey, BlobStorageService blobService)
        {
            var memoryStream = new MemoryStream(); // Tworzymy strumieñ bez using
            try
            {
                using (var writer = new PdfWriter(memoryStream))
                using (var pdf = new PdfDocument(writer))
                {
                    writer.SetCloseStream(false);
                    var document = new Document(pdf);

                    document.SetFont(iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD));
                    document.Add(new Paragraph($"User ID: {survey.UserId}"));
                    document.Add(new Paragraph($"Survey Date: {survey.ResponseDate:yyyy-MM-dd}"));
                    document.Add(new Paragraph($"Survey Hour: {survey.SurveyHour}"));
                    document.Add(new Paragraph($"General Well-Being: {survey.GeneralWellBeingRating}"));
                    document.Add(new Paragraph($"Headache: {survey.HeadacheRating}"));
                    document.Add(new Paragraph($"Abdominal Pain: {survey.AbdominalPainRating}"));
                    document.Add(new Paragraph($"Nausea: {survey.NauseaRating}"));
                    document.Add(new Paragraph($"Fatigue: {survey.FatigueRating}"));
                    document.Add(new Paragraph($"Stress: {survey.StressRating}"));
                    document.Add(new Paragraph($"Energy Level: {survey.EnergyRating}"));
                    document.Add(new Paragraph($"Comments: {survey.AdditionalComments}"));

                    document.Close(); 
                }
                if (!memoryStream.CanSeek)
                {
                    throw new InvalidOperationException("Stream is not seekable.");
                }
                memoryStream.Seek(0, SeekOrigin.Begin); 
                var fileName = $"Survey_{survey.UserId}_{survey.PatientSurveyId}.pdf";
                return await blobService.UploadFileAsync(memoryStream, fileName); 
            }
            finally
            {
                memoryStream.Dispose(); 
            }
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

            var pdfUrl = await GenerateSurveyPdfAsync(PatientSurvey, _blobStorageService);
            string fileName = Path.GetFileName(new Uri(pdfUrl).LocalPath);
            PatientSurvey.FileName = fileName;
            _context.PatientSurveys.Add(PatientSurvey);
            await _context.SaveChangesAsync();
            
            TempData["PdfUrl"] = pdfUrl;
            return RedirectToPage("/Patient/MySurvey");
        }

			
		
	}

    


}
