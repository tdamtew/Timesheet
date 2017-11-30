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
        TimeLog GetTimesheet(DateTime startDate, DateTime endDate, 
            string userId = null, int jobId = 0);
        ApplicationUser GetUser(string userId);
        Job GetJob(int Id);
        int AddUser(ApplicationUser user);
        int UpdateUser(ApplicationUser user);
        int RemoveUser(string userId);
        int AddJob(Job job);
        int UpdateJob(Job job);
        int RemoveJob(int Id);
        int AddExpense(Expense expense);
        int AddMileage(Mileage mileage);
        int AddHour(Hour hour);
        int UpdateExpense(Expense expense);
        int UpdateMileage(Mileage mileage);
        int UpdateHour(Hour hour);
        int RemoveExpense(int Id);
        int RemoveMileage(int Id);
        int RemoveHour(int Id);
    }
}
