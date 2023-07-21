using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MP3BreadLoader.Models
{
	public class AudioConfigModel
    {
        [JsonPropertyName("episodeid"), JsonProperty("episodeid")]
        public int Episodeid { get; set; }
        [JsonPropertyName("content"), JsonProperty("content")]
        public List<AudioConfigContentModel> Content { get; set; }
		public AudioConfigModel()
		{
            Episodeid = 1234;
            Content = new List<AudioConfigContentModel>();
		}
    }

    public class AudioConfigContentModel
    {
        [JsonPropertyName("x"), JsonProperty("x")]
        public bool IsFamilyFilter { get; set; }

        [JsonPropertyName("name"), JsonProperty("name")]
        public string Name { get; set; }

        [JsonPropertyName("short"), JsonProperty("short")]
        public string Short { get; set; }

        [JsonPropertyName("id"), JsonProperty("id")]
        public int Id { get; set; }

        [JsonPropertyName("categories"), JsonProperty("categories")]
        public List<string> Categories { get; set; }
    }
}
