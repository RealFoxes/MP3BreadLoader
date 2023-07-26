using Microsoft.AspNetCore.Mvc;
using MP3BreadLoader.Core.Services;
using MP3BreadLoader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MP3BreadLoader.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AudioContentController : ControllerBase
	{
		private AudioLoaderService audioLoaderService;
		public AudioContentController(AudioLoaderService audioLoaderService)
		{
			this.audioLoaderService = audioLoaderService;
		}

		// GET: api/<AudioContentController>
		[HttpGet]
		public IEnumerable<AudioConfigContentModel> Get() => audioLoaderService.GetAudioConfig().Content;


		// GET api/<AudioContentController>/5
		[HttpGet("{id}")]
		public ActionResult Get(int id)
		{
			var audio = audioLoaderService.GetAudio(id);
			return File(audio, "audio/ogg", $"{id}.ogg");
		}

		// DELETE api/<AudioContentController>/5
		[HttpDelete("{id}")]
		public void Delete(int id) => audioLoaderService.DeleteAudio(id);
	}
}
