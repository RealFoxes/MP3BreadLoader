using Microsoft.Extensions.Configuration;
using MP3BreadLoader.Core.Services;
using MP3BreadLoader.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MP3BreadLoader.Core.Services
{
	public class AudioLoaderService
	{
		private readonly AudioHandlerService audioHandlerService;
		private AudioConfigModel audioConfigModel;
		private readonly string earWaxBasePath;
		private readonly string audioConfigPath;
		private const string audioRelativePath = "EarwaxAudio/Audio/";
		private const string audioSpectrumRelativePath = "EarwaxAudio/Spectrum/";

		public AudioLoaderService(IConfiguration configuration, AudioHandlerService audioHandlerService)
		{
			this.audioHandlerService = audioHandlerService;

			earWaxBasePath = configuration.GetValue<string>("JackboxEarwaxContentPath");
			audioConfigPath = earWaxBasePath + "EarwaxAudio.jet";
			var rawJson = File.ReadAllText(audioConfigPath);
			audioConfigModel = JsonConvert.DeserializeObject<AudioConfigModel>(rawJson);
		}



		public AudioConfigModel GetAudioConfig() => audioConfigModel;
		
		public void AddAudio(string audioName, string audioShortName, bool isFamilyFilter, byte[] audioOgg)
		{
			int id = GetUniqId();
			var fullPath = earWaxBasePath + audioRelativePath + id + ".ogg";
			File.WriteAllBytes(fullPath, audioOgg);

			var frequenciesData = audioHandlerService.GetFrequenciesData(audioOgg);
			var rawJson = JsonConvert.SerializeObject(frequenciesData, Formatting.Indented);
			var frequenciesFullPath = earWaxBasePath + audioSpectrumRelativePath + id + ".jet";
			File.WriteAllText(frequenciesFullPath, rawJson);

			var audioConfigContent = new AudioConfigContentModel();
			audioConfigContent.Id = id;
			audioConfigContent.Name = audioName;
			audioConfigContent.Short = audioShortName;
			audioConfigContent.IsFamilyFilter = isFamilyFilter;
			audioConfigContent.Categories = new List<string> { "household" };
			audioConfigModel.Content.Add(audioConfigContent);

			UpdateConfig();
		}
		public byte[] GetAudio(int id)
		{
			var fullPath = earWaxBasePath + audioRelativePath + id + ".ogg";
			if (!File.Exists(fullPath))
				throw new FileNotFoundException();
			var audio = File.ReadAllBytes(fullPath);
			return audio;
		}
		public void DeleteAudio(int id)
		{
			var fullPath = earWaxBasePath + audioRelativePath + id + ".ogg";
			if (!File.Exists(fullPath))
				throw new FileNotFoundException();
			File.Delete(fullPath);

			var frequenciesFullPath = earWaxBasePath + audioSpectrumRelativePath + id + ".jet";
			if (!File.Exists(frequenciesFullPath))
				throw new FileNotFoundException();
			File.Delete(frequenciesFullPath);

			var item = audioConfigModel.Content.Single(x => x.Id == id);
			audioConfigModel.Content.Remove(item);

			UpdateConfig();
		}

		private int GetUniqId()
		{
			int id = 0;
			id = audioConfigModel.Content.Last().Id + 1;
			while (audioConfigModel.Content.Select(x => x.Id).Contains(id))
				id++;

			return id;
			
		}
		private void UpdateConfig()
		{
			var rawJson = JsonConvert.SerializeObject(audioConfigModel, Formatting.Indented);
			File.WriteAllText(audioConfigPath, rawJson);
		}
	}
}
