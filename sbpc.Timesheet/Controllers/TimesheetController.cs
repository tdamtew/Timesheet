using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using sbpc.Timesheet.Models;
using sbpc.Timesheet.Data;
using System;
using AutoMapper;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using sbpc.Timesheet.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace sbpc.Timesheet.Controllers
{
    [Authorize]
    public class TimesheetController : Controller
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimesheetController(ITimesheetRepository timesheetRepository,
            IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            ViewBag.date = DateTime.Now;
            return View();
        }

        public IActionResult GetDate(DateTime date)
        {
            return ViewComponent("TimesheetWidget", new { userName = _userManager.GetUserName(User), dateTime = date });
        }

        #region manage your hours
        public IActionResult EditHour(int Id, DateTime date)
        {
            return ViewComponent("HourWidget", new { hourId = Id, date = date });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveHour(HourViewModel hour)
        {
            if (ModelState.IsValid)
            {
                hour.UserId = _userManager.GetUserName(User);
                var data = _mapper.Map<Hour>(hour);
                _timesheetRepository.AddorUpdateHour(data);
            }
            return ViewComponent("TimesheetWidget", new { userName = hour.UserId, dateTime = hour.Date });
        }

        [HttpPost]
        public IActionResult DeleteHour(int Id, DateTime date)
        {
            _timesheetRepository.RemoveHour(Id);
            return ViewComponent("TimesheetWidget", new { userName = _userManager.GetUserName(User), dateTime = date });
        }

        #endregion

        #region manage your expense
        public IActionResult EditExpense(int Id, DateTime date)
        {
            return ViewComponent("ExpenseWidget", new { expenseId = Id, date = date });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveExpense(ExpenseViewModel expense)
        {
            if (ModelState.IsValid)
            {
                expense.UserId = _userManager.GetUserName(User);
                var data = _mapper.Map<Expense>(expense);
                _timesheetRepository.AddorUpdateExpense(data);
            }
            return ViewComponent("TimesheetWidget", new { userName = expense.UserId, dateTime = expense.Date });
        }

        [HttpPost]
        public IActionResult DeleteExpense(int Id, DateTime date)
        {
            _timesheetRepository.RemoveExpense(Id);
            return ViewComponent("TimesheetWidget", new { userName = _userManager.GetUserName(User), dateTime = date });
        }
        #endregion

        #region manage your mileage
        public IActionResult EditMileage(int Id, DateTime date)
        {
            return ViewComponent("MileageWidget", new { mileageId = Id, date = date });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveMileage(MileageViewModel mileage)
        {
            if (ModelState.IsValid)
            {
                mileage.UserId = _userManager.GetUserName(User);
                var data = _mapper.Map<Mileage>(mileage);
                _timesheetRepository.AddorUpdateMileage(data);
            }
            return ViewComponent("TimesheetWidget", new { userName = mileage.UserId, dateTime = mileage.Date });
        }

        [HttpPost]
        public IActionResult DeleteMileage(int Id, DateTime date)
        {
            _timesheetRepository.RemoveMileage(Id);
            return ViewComponent("TimesheetWidget", new { userName = _userManager.GetUserName(User), dateTime = date });
        }
        #endregion
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
