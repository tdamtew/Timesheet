using System;

namespace sbpc.Timesheet.Data.Entity
{
    public class Mileage 
    {
        public int Id { get; set; }
        public float mile { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public string EmployeeName { get; set; }
        public string JobName { get; set; }
        public Decimal CalculatedCost { get; set; }
    }
}
