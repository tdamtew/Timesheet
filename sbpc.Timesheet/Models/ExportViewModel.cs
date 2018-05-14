using System;

namespace sbpc.Timesheet.Models
{
    public class ExportViewModel
    {
        public DateTime Date { get; set; }
        public string Job { get; set; }
        public string Employee { get; set; }
        public string Item { get; set; }
        public string PayableItem { get; set; }
        public float Duration { get; set; }
        public string Project { get; set; }
        public string Note { get; set; }
        public int BillingStatus { get; set; }
        public bool IsExported { get; set; }
    }

    public class ExportExpenseViewModel
    {
        public DateTime Date { get; set; }
        public string Job { get; set; }
        public string Employee { get; set; }
        public string Category { get; set; }
        public Decimal Amount { get; set; }
        public string Method { get; set; }
        public string Note { get; set; }
    }
}
