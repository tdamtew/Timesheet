using sbpc.Timesheet.Data.Entity;
using sbpc.Timesheet.Data.Model;
using System;
using System.Linq;
using System.Collections.Generic;

namespace sbpc.Timesheet.Data.Repository
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly ApplicationDbContext _timesheetDbContext;
        public TimesheetRepository(ApplicationDbContext timesheetDbContext)
        {
            _timesheetDbContext = timesheetDbContext;
        }
        public int AddExpense(Expense expense)
        {
            _timesheetDbContext.Expenses.Add(expense);
            return _timesheetDbContext.SaveChanges();
        }

        public int AddHour(Hour hour)
        {
            _timesheetDbContext.Hours.Add(hour);
            return _timesheetDbContext.SaveChanges();
        }

        public int AddJob(Job job)
        {
            _timesheetDbContext.Jobs.Add(job);
            return _timesheetDbContext.SaveChanges();
        }

        public int AddMileage(Mileage mileage)
        {
            _timesheetDbContext.Mileages.Add(mileage);
            return _timesheetDbContext.SaveChanges();
        }

        public int AddUser(ApplicationUser user)
        {
            _timesheetDbContext.Users.Add(user);
            return _timesheetDbContext.SaveChanges();
        }

        public IEnumerable<Job> GetAllJobs() => _timesheetDbContext.Jobs;

        public IEnumerable<ApplicationUser> GetAllUsers() => _timesheetDbContext.Users;

        public Job GetJob(int Id) => _timesheetDbContext.Jobs.First(x => x.Id == Id);

        public TimeLog GetTimesheet(DateTime startDate, DateTime endDate, string userId = null, int jobId = 0)
        {
            var expenses = _timesheetDbContext.Expenses
                                .Where(x => x.Date >= startDate && x.Date <= endDate
                                        && string.IsNullOrEmpty(userId) ? true : x.UserId == userId
                                        && jobId == 0 ? true : x.JobId == jobId);
            var hours = _timesheetDbContext.Hours
                                .Where(x => x.Date >= startDate && x.Date <= endDate
                                        && string.IsNullOrEmpty(userId) ? true : x.UserId == userId
                                        && jobId == 0 ? true : x.JobId == jobId);
            var mileages = _timesheetDbContext.Mileages
                                .Where(x => x.Date >= startDate && x.Date <= endDate
                                        && string.IsNullOrEmpty(userId) ? true : x.UserId == userId
                                        && jobId == 0 ? true : x.JobId == jobId);
            return new TimeLog { Expenses = expenses, Hours = hours, Mileages = mileages };
        }

        public ApplicationUser GetUser(string userId) => _timesheetDbContext.Users.First(x => x.Id == userId);

        public int RemoveExpense(int Id)
        {
            var expense = _timesheetDbContext.Expenses.First(x => x.Id == Id);
            if (expense != null)
                _timesheetDbContext.Expenses.Remove(expense);
            return _timesheetDbContext.SaveChanges();
        }

        public int RemoveHour(int Id)
        {
            var hour = _timesheetDbContext.Hours.First(x => x.Id == Id);
            if (hour != null)
                _timesheetDbContext.Hours.Remove(hour);
            return _timesheetDbContext.SaveChanges();
        }

        public int RemoveJob(int Id)
        {
            var Job = _timesheetDbContext.Jobs.First(x => x.Id == Id);
            if (Job != null)
                _timesheetDbContext.Jobs.Remove(Job);
            return _timesheetDbContext.SaveChanges();
        }

        public int RemoveMileage(int Id)
        {
            var mileage = _timesheetDbContext.Mileages.First(x => x.Id == Id);
            if (mileage != null)
                _timesheetDbContext.Mileages.Remove(mileage);
            return _timesheetDbContext.SaveChanges();
        }

        public int RemoveUser(string userId)
        {
            var user = _timesheetDbContext.Users.First(x => x.Id == userId);
            if (user != null)
                _timesheetDbContext.Users.Remove(user);
            return _timesheetDbContext.SaveChanges();
        }

        public int UpdateExpense(Expense expense)
        {
            _timesheetDbContext.Expenses.Update(expense);
            return _timesheetDbContext.SaveChanges();
        }

        public int UpdateHour(Hour hour)
        {
            _timesheetDbContext.Hours.Update(hour);
            return _timesheetDbContext.SaveChanges();
        }

        public int UpdateJob(Job job)
        {
            _timesheetDbContext.Jobs.Update(job);
            return _timesheetDbContext.SaveChanges();
        }

        public int UpdateMileage(Mileage mileage)
        {
            _timesheetDbContext.Mileages.Update(mileage);
            return _timesheetDbContext.SaveChanges();
        }

        public int UpdateUser(ApplicationUser user)
        {
            _timesheetDbContext.Users.Update(user);
            return _timesheetDbContext.SaveChanges();
        }
    }
}
