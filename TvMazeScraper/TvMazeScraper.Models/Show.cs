using System.Collections.Generic;
using Newtonsoft.Json;

namespace TvMazeScraper.Models
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("cast")]
        public IEnumerable<Cast> Cast { get; set; }
    }
}
