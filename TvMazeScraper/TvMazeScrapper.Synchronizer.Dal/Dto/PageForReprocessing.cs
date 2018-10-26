using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvMazeScraper.Synchronizer.Dal.Dto
{
    [Table("PagesProcessingStatuses")]
    public class PageProcessingStatus 
    {
        [Key]
        public int Id { get; set; }

        public int AttemptToProcess { get; set; }

        public string LastError { get; set; }

        public int Status { get; set; }
    }
}
