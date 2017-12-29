using System;
using System.Collections.Generic;
using System.Text;

namespace sbpc.Timesheet.Data.Entity
{
    public class Mileage 
    {
        public int Id { get; set; }
        public int mile { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public string EmployeeName { get; set; }
        public string JobName { get; set; }
    }
}
