using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace StackExchangeTags.Models
{
    public class StackExchangeTag
    {
        [JsonProperty("has_synonyms")]
        public bool HasSynonyms { get; set; }
        [JsonProperty("is_moderator_only")]
        public bool IsModeratorOnly { get; set; }
        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }
        [JsonProperty("count")]
        public long Count { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}