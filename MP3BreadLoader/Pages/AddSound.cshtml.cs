using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MP3BreadLoader.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MP3BreadLoader.Pages
{
	public class AddSoundModel : PageModel
    {
        [BindProperty]
        public IFormFile OggFile { get; set; }

        [BindProperty]
        public string AudioName { get; set; }

        [BindProperty]
        public string AudioShortName { get; set; }
        [BindProperty]
        public bool IsFamilyFilter { get; set; }

        public bool IsUploaded { get; private set; }

		private readonly ILogger<AddSoundModel> logger;
		private readonly AudioLoaderService audioLoaderService;

		public AddSoundModel(ILogger<AddSoundModel> logger, AudioLoaderService audioLoaderService)
		{
			this.logger = logger;
            this.audioLoaderService = audioLoaderService;
        }

		public void OnGet()
		{
		}

        public async Task<IActionResult> OnPostAsync()
        {
            if (OggFile == null || OggFile.Length == 0)
            {
                ModelState.AddModelError("OggFile", "Please select an OGG file.");
                return Page();
            }

            // Access additional form data
            var audioName = AudioName;
            var audioShortName = AudioShortName;
            var isFamilyFilter = IsFamilyFilter;

            // Process the uploaded OGG file and get frequencies data
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await OggFile.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            audioLoaderService.AddAudio(audioName, audioShortName, isFamilyFilter, fileBytes);

            IsUploaded = true;
            return Page();
        }
    }
}
