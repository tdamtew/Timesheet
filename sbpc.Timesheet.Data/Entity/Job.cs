﻿namespace sbpc.Timesheet.Data.Entity
{
    public class Job
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal OverTimeRate { get; set; }
        public double CostPerMile { get; set; }
        public bool Active { get; set; }
    }
}
