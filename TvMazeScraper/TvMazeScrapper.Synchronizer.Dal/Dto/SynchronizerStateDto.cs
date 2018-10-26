using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvMazeScraper.Synchronizer.Dal.Dto
{
    [Table("SynchronizerState")]
    public class SynchronizerStateDto
    {
        public DateTime? LastUpdatedDate { get; set; }
        public int StateId { get; set; }
    }
}
