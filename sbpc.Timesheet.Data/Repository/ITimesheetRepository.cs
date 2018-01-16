using sbpc.Timesheet.Data.Entity;
using sbpc.Timesheet.Data.Model;
using System;
using System.Collections.Generic;

namespace sbpc.Timesheet.Data
{
    public interface ITimesheetRepository
    {
        IEnumerable<ApplicationUser> GetAllUsers();
        IEnumerable<Job> GetAllJobs();

        TimeLog GetTimesheet(DateTime startDate, DateTime endDate, string userId = "", string jobName = "");
        ApplicationUser GetUser(string userId);
        Job GetJob(int Id);
        int UpdateUser(ApplicationUser user);
        Hour GetHour(int Id);
        IEnumerable<Hour> GetHours(DateTime startDate, DateTime endDate, string employee = "");
        int AddorUpdateJob(Job job);
        int RemoveJob(int Id);
        int AddorUpdateExpense(Expense expense);
        int AddorUpdateMileage(Mileage mileage);
        int AddorUpdateHour(Hour hour);
        Expense GetExpense(int Id);
        Mileage GetMileage(int Id);
        int RemoveExpense(int Id);
        int RemoveMileage(int Id);
        int RemoveHour(int Id);
    }
}
