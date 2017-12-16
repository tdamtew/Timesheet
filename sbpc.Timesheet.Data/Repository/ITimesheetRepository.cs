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
        TimeLog GetUserTimesheet(DateTime date, string userId);
        ApplicationUser GetUser(string userId);
        Job GetJob(int Id);
        int AddUser(ApplicationUser user);
        int UpdateUser(ApplicationUser user);
        int RemoveUser(string userId);

        Hour GetHour(int Id);
        int AddJob(Job job);
        int UpdateJob(Job job);
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
