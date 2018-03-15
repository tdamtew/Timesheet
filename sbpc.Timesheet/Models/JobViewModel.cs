using System.ComponentModel.DataAnnotations;

namespace sbpc.Timesheet.Models.AdminViewModels
{
    public class JobViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Job Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Overtime Rate ")]
        public decimal OverTimeRate { get; set; }

        [Required]
        [Display(Name = "Cost Per Mile")]
        [DataType(DataType.Currency)]
        public double CostPerMile { get; set; }

        public bool Active { get; set; }

    }
}
