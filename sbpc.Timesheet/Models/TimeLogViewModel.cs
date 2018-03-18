using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sbpc.Timesheet.Models
{
    public class TimesheetViewModel
    {
        public DateTime date { get; set; }
        public IEnumerable<ExpenseViewModel> Expenses { get; set; }
        public IEnumerable<HourViewModel> Hours { get; set; }
        public IEnumerable<MileageViewModel> Mileages { get; set; }
    }
    public class TimeLogViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string JobName { get; set; }

        public string EmployeeName { get; set; }

        public string Note { get; set; }
    }

    public class ExpenseViewModel : TimeLogViewModel
    {
        [Required]
        public Decimal Amount { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Method { get; set; }

    }

    public class HourViewModel : TimeLogViewModel
    {
        [Required]
        public float Hours { get; set; }
        public bool IsTravel { get; set; }
        public bool Billable { get; set; }
        public float OTHours { get; set; }

    }

    public class MileageViewModel : TimeLogViewModel
    {
        [Required]
        public float mile { get; set; }
        public decimal CalculatedCost { get; set; }
    }
}
