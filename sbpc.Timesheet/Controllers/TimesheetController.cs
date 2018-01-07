﻿using Microsoft.AspNetCore.Mvc;
using sbpc.Timesheet.Models;
using sbpc.Timesheet.Data;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using sbpc.Timesheet.Data.Entity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace sbpc.Timesheet.Controllers
{
    [Authorize]
    public class TimesheetController : Controller
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private string _currentEmployee;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimesheetController(ITimesheetRepository timesheetRepository,
            IMapper mapper, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _timesheetRepository = timesheetRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        private string CurrentEmployee()
        {
            if (!string.IsNullOrEmpty(_currentEmployee)) return _currentEmployee;
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            _currentEmployee = $"{user.FirstName} {user.LastName}";
            return _currentEmployee;
        }

        //get timesheet for current employee
        public IActionResult Index()
        {
            if (_configuration.GetSection("Data:TimesheetAdmin").Get<string[]>().Contains(User.Identity.Name))
                return RedirectToAction(nameof(Admin));
            ViewBag.date = DateTime.Now;
            ViewBag.currentUser = CurrentEmployee();
            return View();
        }

        //admin timesheet entry for another employee.
        [Authorize(policy: "TimesheetAdminRole")]
        public IActionResult Admin()
        {
            var employeesData = _timesheetRepository.GetAllUsers().Where(x => x.IsEnabled);
            var currentUser = CurrentEmployee();
            if (employeesData != null)
            {
                var employeeList = employeesData.Select(x => new SelectListItem
                {
                    Value = $"{x.FirstName} {x.LastName}",
                    Text = $"{x.FirstName} {x.LastName}",
                    Selected = string.Compare($"{x.FirstName} {x.LastName}", currentUser, true) == 0
                }).ToList();

                ViewBag.employeeList = employeeList;
            }
            ViewBag.date = DateTime.Now;
            ViewBag.currentUser = currentUser;
            return View("Index");
        }

        public IActionResult GetDate(DateTime date, string employee)
        {
            return ViewComponent("TimesheetWidget", new { userName = string.IsNullOrEmpty(employee) ? CurrentEmployee() : employee, dateTime = date });
        }

        #region manage your hours
        public IActionResult EditHour(int Id, DateTime date)
        {
            return ViewComponent("HourWidget", new { hourId = Id, date = date });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveHour(HourViewModel hour, string employee)
        {
            if (ModelState.IsValid)
            {
                hour.EmployeeName = string.IsNullOrEmpty(employee) ? CurrentEmployee() : employee;
                if(hour.IsTravel)
                {
                    if(!string.IsNullOrEmpty(hour.Note))
                    {
                        hour.Note = hour.Note.Contains("Travel") ? hour.Note : $"Travel {hour.Note}";
                    }
                    else
                    {
                        hour.Note = "Travel";
                    }
                }
                var data = _mapper.Map<Hour>(hour);
                _timesheetRepository.AddorUpdateHour(data);
            }
            return ViewComponent("TimesheetWidget", new { userName = hour.EmployeeName, dateTime = hour.Date });
        }

        [HttpPost]
        public IActionResult DeleteHour(int Id, DateTime date, string employee)
        {
            _timesheetRepository.RemoveHour(Id);
            return ViewComponent("TimesheetWidget", new { userName = string.IsNullOrEmpty(employee) ? CurrentEmployee() : employee, dateTime = date });
        }

        #endregion

        #region manage your expense
        public IActionResult EditExpense(int Id, DateTime date)
        {
            return ViewComponent("ExpenseWidget", new { expenseId = Id, date = date });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveExpense(ExpenseViewModel expense, string employee)
        {
            if (ModelState.IsValid)
            {
                expense.EmployeeName = string.IsNullOrEmpty(employee) ? CurrentEmployee() : employee;
                var data = _mapper.Map<Expense>(expense);
                _timesheetRepository.AddorUpdateExpense(data);
            }
            return ViewComponent("TimesheetWidget", new { userName = expense.EmployeeName, dateTime = expense.Date });
        }

        [HttpPost]
        public IActionResult DeleteExpense(int Id, DateTime date, string employee)
        {
            _timesheetRepository.RemoveExpense(Id);
            return ViewComponent("TimesheetWidget", new { userName = string.IsNullOrEmpty(employee) ? CurrentEmployee() : employee, dateTime = date });
        }
        #endregion

        #region manage your mileage
        public IActionResult EditMileage(int Id, DateTime date)
        {
            return ViewComponent("MileageWidget", new { mileageId = Id, date = date });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveMileage(MileageViewModel mileage, string employee)
        {
            if (ModelState.IsValid)
            {
                mileage.EmployeeName = string.IsNullOrEmpty(employee) ? CurrentEmployee() : employee;
                var data = _mapper.Map<Mileage>(mileage);
                _timesheetRepository.AddorUpdateMileage(data);
            }
            return ViewComponent("TimesheetWidget", new { userName = mileage.EmployeeName, dateTime = mileage.Date });
        }

        [HttpPost]
        public IActionResult DeleteMileage(int Id, DateTime date, string employee)
        {
            _timesheetRepository.RemoveMileage(Id);
            return ViewComponent("TimesheetWidget", new { userName = string.IsNullOrEmpty(employee) ? CurrentEmployee() : employee, dateTime = date });
        }
        #endregion
    }
}
