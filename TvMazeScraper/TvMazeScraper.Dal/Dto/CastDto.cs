using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvMazeScraper.Dal.Dto
{
    [Table("Cast")]
    public class CastDto 
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        [System.ComponentModel.DataAnnotations.Key]
        public int ShowId { get; set; }
    }
}
