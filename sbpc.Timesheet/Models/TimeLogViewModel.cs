using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sbpc.Timesheet.Models
{
    public class TimesheetViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<ExpenseViewModel> Expenses { get; set; }
        public IEnumerable<HourViewModel> Hours { get; set; }
        public IEnumerable<MileageViewModel> Mileages { get; set; }
    }
    public class TimeLogViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public int JobId { get; set; }

        [Required]
        public int UserId { get; set; }

        public string Note { get; set; }
    }

    public class ExpenseViewModel : TimeLogViewModel
    {
        [Required]
        [DataType(DataType.Currency)]
        public Decimal Amount { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int MethodId { get; set; }

    }

    public class HourViewModel : TimeLogViewModel
    {
        [Required]
        public int Hours { get; set; }
        [Required]
        public int ItemId { get; set; }
    }

    public class MileageViewModel : TimeLogViewModel
    {
        [Required]
        public int mile { get; set; }
    }
}
