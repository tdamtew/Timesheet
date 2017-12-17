using sbpc.Timesheet.Data.Entity;
using sbpc.Timesheet.Data.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

        public int AddorUpdateMileage(Mileage mileage)
        {
            if (mileage.Id == 0)
                _timesheetDbContext.Mileages.Add(mileage);
            else
            {
                if (!_timesheetDbContext.Mileages.Any(x => x.Id == mileage.Id))
                {
                    mileage.Id = 0;
                    _timesheetDbContext.Mileages.Add(mileage);
                }
                else
                {
                    _timesheetDbContext.Mileages.Update(mileage);
                }
            }
            return _timesheetDbContext.SaveChanges();
        }

        public int AddUser(ApplicationUser user)
        {
            _timesheetDbContext.Users.Add(user);
            return _timesheetDbContext.SaveChanges();
        }

        public int AddorUpdateExpense(Expense expense)
        {
            if (expense.Id == 0)
                _timesheetDbContext.Expenses.Add(expense);
            else
            {
                if (!_timesheetDbContext.Expenses.Any(x => x.Id == expense.Id))
                {
                    expense.Id = 0;
                    _timesheetDbContext.Expenses.Add(expense);
                }
                else
                {
                    _timesheetDbContext.Expenses.Update(expense);
                }
            }
            return _timesheetDbContext.SaveChanges();
        }

        public int AddorUpdateHour(Hour hour)
        {
            if (hour.Id == 0)
                _timesheetDbContext.Hours.Add(hour);
            else
            {
                if (!_timesheetDbContext.Hours.Any(x => x.Id == hour.Id))
                {
                    hour.Id = 0;
                    _timesheetDbContext.Hours.Add(hour);
                }
                else
                {
                    _timesheetDbContext.Hours.Update(hour);
                }
            }
            return _timesheetDbContext.SaveChanges();
        }

        public IEnumerable<Job> GetAllJobs() => _timesheetDbContext.Jobs;

        public IEnumerable<ApplicationUser> GetAllUsers() => _timesheetDbContext.Users;

        public Job GetJob(int Id) => _timesheetDbContext.Jobs.First(x => x.Id == Id);

        public TimeLog GetTimesheet(DateTime startDate, DateTime endDate, string userId = "", string jobName = "")
        {
            var expenses = _timesheetDbContext.Expenses
                 .Where(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date);
            var hours = _timesheetDbContext.Hours
                 .Where(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date);
            var mileages = _timesheetDbContext.Mileages
                 .Where(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date);
            if (!string.IsNullOrEmpty(userId))
            {
                expenses = expenses.Where(x => x.UserId == userId);
                hours = hours.Where(x => x.UserId == userId);
                mileages = mileages.Where(x => x.UserId == userId);
            }
            if (!string.IsNullOrEmpty(jobName))
            {
                expenses = expenses.Where(x => x.JobName == jobName);
                hours = hours.Where(x => x.JobName == jobName);
                mileages = mileages.Where(x => x.JobName == jobName);
            }

            return new TimeLog
            {
                Expenses = expenses.Any() ? expenses.ToList() : null,
                Hours = hours.Any() ? hours.ToList() : null,
                Mileages = mileages.Any() ? mileages.ToList() : null
            };
        }

        public ApplicationUser GetUser(string userId) => _timesheetDbContext.Users.First(x => x.UserName == userId);

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
            var user = _timesheetDbContext.Users.First(x => x.UserName == userId);
            if (user != null)
                _timesheetDbContext.Users.Remove(user);
            return _timesheetDbContext.SaveChanges();
        }

        public int UpdateJob(Job job)
        {
            _timesheetDbContext.Jobs.Update(job);
            return _timesheetDbContext.SaveChanges();
        }

        public Expense GetExpense(int Id) => _timesheetDbContext.Expenses.First(x => x.Id == Id);

        public Mileage GetMileage(int Id) => _timesheetDbContext.Mileages.First(x => x.Id == Id);

        public int UpdateUser(ApplicationUser user)
        {
            var updatedUser = _timesheetDbContext.Users.First(x => x.UserName == user.UserName);
            if (updatedUser == null) throw new ApplicationException($"Unable to find the user : {user.UserName}");
            updatedUser.FirstName = user.FirstName;
            updatedUser.LastName = user.LastName;
            updatedUser.MiddleName = user.MiddleName;
            updatedUser.PhoneNumber = user.PhoneNumber;
            _timesheetDbContext.Users.Update(updatedUser);
            return _timesheetDbContext.SaveChanges();
        }

        public Hour GetHour(int Id) => _timesheetDbContext.Hours.First(x => x.Id == Id);

    }
}
