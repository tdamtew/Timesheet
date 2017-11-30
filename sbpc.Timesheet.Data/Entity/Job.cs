using System;
using System.Collections.Generic;
using System.Text;

namespace sbpc.Timesheet.Data.Entity
{
    public class Job
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal OverTimeRate { get; set; }
        public double CostPerMile { get; set; }
    }
    public class Method
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BillingType { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
