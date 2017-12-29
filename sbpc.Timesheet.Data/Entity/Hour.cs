using System;
using System.Collections.Generic;
using System.Text;

namespace sbpc.Timesheet.Data.Entity
{
    public class Hour
    {
        public int Id { get; set; }
        public int Hours { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public bool IsTravel { get; set; }
        public string EmployeeName { get; set; }
        public string JobName { get; set; }
    }
}
