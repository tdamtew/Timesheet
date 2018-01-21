﻿using sbpc.Timesheet.Data.Entity;
using sbpc.Timesheet.Data.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

        public bool DoesEmployeeExists(string fullName) => _timesheetDbContext.Users.Any(x => string.Compare($"{x.FirstName} {x.MiddleName} {x.LastName}", fullName, true) == 0);
        public int UpdateTempPasswordFlag(string userId, bool set)
        {
            var user = _timesheetDbContext.Users.FirstOrDefault(x => x.UserName == userId);
            if (user == null) return 0;
            user.TempPassword = set;
            _timesheetDbContext.Users.Update(user);
            return _timesheetDbContext.SaveChanges();
        }
        public int UpdateUser(ApplicationUser user)
        {
            var updateUser = _timesheetDbContext.Users.First(x => x.UserName == user.UserName);
            if (updateUser == null) throw new ApplicationException($"Unable to find the user : {user.UserName}");
            var expenses = _timesheetDbContext.Expenses.Where(x => x.EmployeeName == $"{updateUser.FirstName} {updateUser.MiddleName} {updateUser.LastName}");
            if (expenses != null)
            {
                foreach (var exp in expenses)
                {
                    exp.EmployeeName = $"{user.FirstName} {updateUser.MiddleName} {user.LastName}";
                    _timesheetDbContext.Expenses.Update(exp);
                }
            }
            var hours = _timesheetDbContext.Hours.Where(x => x.EmployeeName == $"{updateUser.FirstName} {updateUser.MiddleName} {updateUser.LastName}");
            if (hours != null)
            {
                foreach (var hour in hours)
                {
                    hour.EmployeeName = $"{user.FirstName} {updateUser.MiddleName} {user.LastName}";
                    _timesheetDbContext.Hours.Update(hour);
                }
            }
            var mileages = _timesheetDbContext.Mileages.Where(x => x.EmployeeName == $"{updateUser.FirstName} {updateUser.MiddleName} {updateUser.LastName}");
            if (mileages != null)
            {
                foreach (var mile in mileages)
                {
                    mile.EmployeeName = $"{user.FirstName} {updateUser.MiddleName} {user.LastName}";
                    _timesheetDbContext.Mileages.Update(mile);
                }
            }
            updateUser.FirstName = user.FirstName;
            updateUser.MiddleName = user.MiddleName;
            updateUser.LastName = user.LastName;
            updateUser.IsEnabled = user.IsEnabled;
            updateUser.PhoneNumber = user.PhoneNumber;
            _timesheetDbContext.Users.Update(updateUser);
            return _timesheetDbContext.SaveChanges();
        }
        #endregion

        #region jobs
        public IEnumerable<Job> GetAllJobs() => _timesheetDbContext.Jobs;
        public Job GetJob(int Id) => _timesheetDbContext.Jobs.FirstOrDefault(x => x.Id == Id);
        public int AddorUpdateJob(Job job)
        {
            if (!_timesheetDbContext.Jobs.Any(x => x.Id == job.Id))
            {
                job.Id = 0;
                var existingJob = _timesheetDbContext.Jobs.FirstOrDefault(x => string.Compare(x.Name, job.Name) == 0);
                if (existingJob != null)
                {
                    existingJob.CostPerMile = job.CostPerMile;
                    existingJob.OverTimeRate = job.OverTimeRate;
                    _timesheetDbContext.Jobs.Update(existingJob);
                    return _timesheetDbContext.SaveChanges();
                }
                _timesheetDbContext.Jobs.Add(job);
            }
            else
            {
                var oldName = _timesheetDbContext.Jobs.AsNoTracking().FirstOrDefault(x => x.Id == job.Id).Name;
                var expenses = _timesheetDbContext.Expenses.Where(x => x.JobName == oldName);
                if (expenses != null)
                {
                    foreach (var exp in expenses)
                    {
                        exp.JobName = job.Name;
                        _timesheetDbContext.Expenses.Update(exp);
                    }
                }
                var hours = _timesheetDbContext.Hours.Where(x => x.JobName == oldName);
                if (hours != null)
                {
                    foreach (var hour in hours)
                    {
                        hour.JobName = job.Name;
                        _timesheetDbContext.Hours.Update(hour);
                    }
                }
                var mileages = _timesheetDbContext.Mileages.Where(x => x.JobName == oldName);
                if (mileages != null)
                {
                    foreach (var mile in mileages)
                    {
                        mile.JobName = job.Name;
                        _timesheetDbContext.Mileages.Update(mile);
                    }
                }
                _timesheetDbContext.Jobs.Update(job);
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
            if (!string.IsNullOrEmpty(employee))
            {
                hours = hours.Where(x => x.EmployeeName == employee);
            }
            return hours;
        }
        public int AddorUpdateHour(Hour hour)
        {
            //get hours for the week.
            var startOfWeek = hour.Date.AddDays((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)hour.Date.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);
            if (hour.IsTravel)
            {
                _timesheetDbContext.Hours.Add(hour);
                return _timesheetDbContext.SaveChanges();
            }
            var weeklyHours = GetHours(startOfWeek, endOfWeek, hour.EmployeeName);
            if (weeklyHours == null || !weeklyHours.Any())
            {
                _timesheetDbContext.Hours.Add(hour);
                return _timesheetDbContext.SaveChanges();
            }
            var workHours = weeklyHours.Where(x => !x.IsTravel);
            if (workHours == null || !workHours.Any())
            {
                _timesheetDbContext.Hours.Add(hour);
                return _timesheetDbContext.SaveChanges();
            }
            var totalHours = workHours.Sum(x => x.Hours);
            if (totalHours >= 40)
            {
                hour.OTHours = hour.Hours;
                _timesheetDbContext.Hours.Add(hour);
                return _timesheetDbContext.SaveChanges();
            }
            if (totalHours + hour.Hours > 40)
                hour.OTHours = (totalHours + hour.Hours) % 40;
            _timesheetDbContext.Hours.Add(hour);
            return _timesheetDbContext.SaveChanges();
        }
        public Hour GetHour(int Id) => _timesheetDbContext.Hours.First(x => x.Id == Id);
        public void RemoveHour(int Id)
        {
            var hour = _timesheetDbContext.Hours.First(x => x.Id == Id);
            if (hour != null)
                _timesheetDbContext.Hours.Remove(hour);
            _timesheetDbContext.SaveChanges();

            //recalculate overtime for the week.
            var startOfWeek = hour.Date.AddDays((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)hour.Date.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);
            var weeklyHours = GetHours(startOfWeek, endOfWeek, hour.EmployeeName);
            if (weeklyHours == null || !weeklyHours.Any()) return;
            var workHours = weeklyHours.Where(x => !x.IsTravel);
            if (workHours == null || !workHours.Any() || !workHours.Any(x => x.OTHours > 0)) return;
            var flaggedHours = workHours.Where(x => x.OTHours > 0).OrderBy(x => x.Date);
            var totalHours = workHours.Sum(x => x.Hours);
            if (totalHours <= 40)
            {
                foreach (var h in flaggedHours)
                {
                    h.OTHours = 0;
                    _timesheetDbContext.Hours.Update(h);
                }
                _timesheetDbContext.SaveChanges();
                return;
            }
            var otHours = totalHours - 40;
            foreach (var h in flaggedHours)
            {
                if (h.OTHours >= otHours)
                {
                    h.OTHours = otHours;
                    _timesheetDbContext.Hours.Update(h);
                    break;
                }
                otHours -= h.OTHours;
                h.OTHours = 0;
                _timesheetDbContext.Hours.Update(h);
            }
            _timesheetDbContext.SaveChanges();
        }

        public void UpdateExportFlag(Hour hour)
        {
            hour.IsExported = true;
            _timesheetDbContext.Hours.Update(hour);
            _timesheetDbContext.SaveChanges();
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
