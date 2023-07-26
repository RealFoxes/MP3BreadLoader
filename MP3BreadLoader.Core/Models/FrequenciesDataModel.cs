using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MP3BreadLoader.Core.Models
{
	public class FrequenciesDataModel
	{
		public int Refresh { get; set; }
		public List<FrequenciesChannelsModel> Frequencies { get; set; }
		public int Peak { get; set; }

	}
	public class FrequenciesChannelsModel
	{
		[JsonPropertyName("left"), JsonProperty("left")]
		public int[] Left { get; set; }
		[JsonPropertyName("right"), JsonProperty("right")]
		public int[] Right { get; set; }
	}
}
