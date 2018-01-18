using System;

namespace sbpc.Timesheet.Data.Entity
{
    public class Hour
    {
        public int Id { get; set; }
        public float Hours { get; set; }
        public float OTHours { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public bool IsTravel { get; set; }
        public string EmployeeName { get; set; }
        public string JobName { get; set; }
        public bool IsExported { get; set; }
        public bool Billable { get; set; }
    }
}
