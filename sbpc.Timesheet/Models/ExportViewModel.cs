using System;

namespace sbpc.Timesheet.Models
{
    public class ExportViewModel
    {
        public DateTime Date { get; set; }
        public string Job { get; set; }
        public string Employee { get; set; }
        public string Item { get; set; }
        public string PItem { get; set; }
        public int Duration { get; set; }
        public string Project { get; set; }
        public string Note { get; set; }
        public int BillingStatus { get; set; }
    }
}
