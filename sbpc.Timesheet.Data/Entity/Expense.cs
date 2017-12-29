using System;

namespace sbpc.Timesheet.Data.Entity
{
    public class Expense
    {
        public int Id { get; set; }
        public Decimal Amount { get; set; }
        public string Note { get; set; }
        public string Category { get; set; }
        public string Method { get; set; }
        public DateTime Date { get; set; }
        public string EmployeeName { get; set; }
        public string JobName { get; set; }
    }
}
