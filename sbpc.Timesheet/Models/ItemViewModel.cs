using System.ComponentModel.DataAnnotations;

namespace sbpc.Timesheet.Models
{
    public class ItemViewModel
    {
        [Required]
        public string Employee { get; set; }
        [Required]
        public string Job { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
