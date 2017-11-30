using sbpc.Timesheet.Data.Entity;
using System;
using System.Collections.Generic;

namespace sbpc.Timesheet.Data.Model
{
    public class TimeLog
    {
        public IEnumerable<Expense> Expenses { get; set; }
        public IEnumerable<Mileage> Mileages { get; set; }
        public IEnumerable<Hour> Hours { get; set; }
    }
}
