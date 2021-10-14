using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace StackExchangeTags.Models
{
    public class StackExchangeTagList<T>
    {
        [JsonProperty("items")]
        public List<T> Items { get; set; }
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }
        [JsonProperty("quota_max")]
        public long QuotaMax { get; set; }
        [JsonProperty("quota_remaining")]
        public long QuotaRemaining { get; set; }
    }
}