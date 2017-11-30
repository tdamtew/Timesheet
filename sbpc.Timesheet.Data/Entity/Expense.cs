using System;
using System.Collections.Generic;
using System.Text;

namespace sbpc.Timesheet.Data.Entity
{
    public class Expense
    {
        public int Id { get; set; }
        public Decimal Amount { get; set; }
        public string Note { get; set; }
        public int CategoryId { get; set; }
        public int MethodId { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public int JobId { get; set; }
    }
}
