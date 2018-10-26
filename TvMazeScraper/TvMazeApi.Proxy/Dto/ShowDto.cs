using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TvMazeApi.Proxy.Dto
{
    public class ShowDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("_embedded")]
        public EmbeddedDto Embedded { get; set; }
    }

    public class EmbeddedDto
    {
        [JsonProperty("cast")]
        public IEnumerable<CastDto> Cast { get; set; }
    }

    public class CastDto
    {
        public PersonDto Person { get; set; }
    }

    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
