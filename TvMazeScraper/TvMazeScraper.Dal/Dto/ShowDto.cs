using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvMazeScraper.Dal.Dto
{
    [Table("Shows")]
    public class ShowDto 
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CastDto> Cast { get; set; }
    }  
}
