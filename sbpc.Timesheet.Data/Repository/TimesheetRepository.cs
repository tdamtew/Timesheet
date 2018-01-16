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

        #region users
        public IEnumerable<ApplicationUser> GetAllUsers() => _timesheetDbContext.Users;
        public ApplicationUser GetUser(string userId) => _timesheetDbContext.Users.FirstOrDefault(x => x.UserName == userId);
        public int UpdateUser(ApplicationUser user)
        {
            var updatedUser = _timesheetDbContext.Users.First(x => x.UserName == user.UserName);
            if (updatedUser == null) throw new ApplicationException($"Unable to find the user : {user.UserName}");
            updatedUser.FirstName = user.FirstName;
            updatedUser.LastName = user.LastName;
            updatedUser.MiddleName = user.MiddleName;
            updatedUser.PhoneNumber = user.PhoneNumber;
            updatedUser.IsEnabled = user.IsEnabled;
            _timesheetDbContext.Users.Update(updatedUser);
            return _timesheetDbContext.SaveChanges();
        }
        #endregion

        #region jobs
        public IEnumerable<Job> GetAllJobs() => _timesheetDbContext.Jobs;
        public Job GetJob(int Id) => _timesheetDbContext.Jobs.FirstOrDefault(x => x.Id == Id);
        public int AddorUpdateJob(Job job)
        {
            if (job.Id == 0)
                _timesheetDbContext.Jobs.Add(job);
            else
            {
                if (!_timesheetDbContext.Jobs.Any(x => x.Id == job.Id))
                {
                    job.Id = 0;
                    _timesheetDbContext.Jobs.Add(job);
                }
                else
                {
                    _timesheetDbContext.Jobs.Update(job);
                }
            }
            return _timesheetDbContext.SaveChanges();
        }
        public int RemoveJob(int Id)
        {
            var Job = _timesheetDbContext.Jobs.First(x => x.Id == Id);
            if (Job != null)
                _timesheetDbContext.Jobs.Remove(Job);
            return _timesheetDbContext.SaveChanges();
        }
        #endregion

        #region timesheet
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
                expenses = expenses.Where(x => x.EmployeeName == userId);
                hours = hours.Where(x => x.EmployeeName == userId);
                mileages = mileages.Where(x => x.EmployeeName == userId);
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
        #endregion

        #region hour

        public IEnumerable<Hour> GetHours(DateTime startDate, DateTime endDate, string employee = "")
        {
            var hours = _timesheetDbContext.Hours.Where(a => a.Date >= startDate.Date && a.Date <= endDate.Date);
            if(!string.IsNullOrEmpty(employee))
            {
                hours = hours.Where(x => x.EmployeeName == employee);
            }
            return hours;
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
        public Hour GetHour(int Id) => _timesheetDbContext.Hours.First(x => x.Id == Id);
        public int RemoveHour(int Id)
        {
            var hour = _timesheetDbContext.Hours.First(x => x.Id == Id);
            if (hour != null)
                _timesheetDbContext.Hours.Remove(hour);
            return _timesheetDbContext.SaveChanges();
        }
        #endregion

        #region expense
        public Expense GetExpense(int Id) => _timesheetDbContext.Expenses.First(x => x.Id == Id);
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
        public int RemoveExpense(int Id)
        {
            var expense = _timesheetDbContext.Expenses.First(x => x.Id == Id);
            if (expense != null)
                _timesheetDbContext.Expenses.Remove(expense);
            return _timesheetDbContext.SaveChanges();
        }

        #endregion

        #region mileage
        public Mileage GetMileage(int Id) => _timesheetDbContext.Mileages.First(x => x.Id == Id);
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
        public int RemoveMileage(int Id)
        {
            var mileage = _timesheetDbContext.Mileages.First(x => x.Id == Id);
            if (mileage != null)
                _timesheetDbContext.Mileages.Remove(mileage);
            return _timesheetDbContext.SaveChanges();
        }

        #endregion
    }
}
