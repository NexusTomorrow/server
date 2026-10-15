using System.ComponentModel.DataAnnotations;

namespace NexusTomorrow.GameTable.Configuration.Model
{
    public class GameTableConfig
    {
        [Required]
        public string GameTablePath { get; set; }
        public CacheConfig Cache { get; set; }
    }
}
