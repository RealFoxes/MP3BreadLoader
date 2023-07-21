using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MP3BreadLoader.Models
{
	public class PromptConfigModel
	{
        [JsonPropertyName("content"), JsonProperty("content")]
        public List<PromptConfigContentModel> Content { get; set; }
		public PromptConfigModel()
		{
            Content = new List<PromptConfigContentModel>();
		}
    }

    public class PromptConfigContentModel
    {
        [JsonPropertyName("id"), JsonProperty("id")]
        public int Id { get; set; }

        [JsonPropertyName("x"), JsonProperty("x")]
        public bool X { get; set; }

        [JsonPropertyName("PromptAudio"), JsonProperty("PromptAudio")]
        public string PromptAudio { get; set; }

        [JsonPropertyName("name"), JsonProperty("name")]
        public string Name { get; set; }
    }
}
