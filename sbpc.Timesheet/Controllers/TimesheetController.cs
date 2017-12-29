using Microsoft.AspNetCore.Mvc;
using sbpc.Timesheet.Models;
using sbpc.Timesheet.Data;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using sbpc.Timesheet.Data.Entity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Controllers
{
    [Authorize]
    public class TimesheetController : Controller
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;
        private string _currentEmployee;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimesheetController(ITimesheetRepository timesheetRepository,
            IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _timesheetRepository = timesheetRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        private string CurrentEmployee()
        {
            if (!string.IsNullOrEmpty(_currentEmployee)) return _currentEmployee;
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            _currentEmployee = $"{user.FirstName} {user.MiddleName} {user.LastName}";
            return _currentEmployee;
        }

        public IActionResult Index()
        {
            ViewBag.date = DateTime.Now;
            ViewBag.currentUser = CurrentEmployee();
            return View();
        }

        public IActionResult GetDate(DateTime date)
        {
            return ViewComponent("TimesheetWidget", new { userName = CurrentEmployee(), dateTime = date });
        }

        #region manage your hours
        public IActionResult EditHour(int Id, DateTime date)
        {
            return ViewComponent("HourWidget", new { hourId = Id, date = date });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveHour(HourViewModel hour, string employeeName)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(employeeName))
                    hour.EmployeeName = CurrentEmployee();
                var data = _mapper.Map<Hour>(hour);
                _timesheetRepository.AddorUpdateHour(data);
            }
            return ViewComponent("TimesheetWidget", new { userName = hour.EmployeeName, dateTime = hour.Date });
        }

        [HttpPost]
        public IActionResult DeleteHour(int Id, DateTime date, string employeeName)
        {
            _timesheetRepository.RemoveHour(Id);
            return ViewComponent("TimesheetWidget", new { userName = string.IsNullOrEmpty(employeeName) ? CurrentEmployee() : employeeName, dateTime = date });
        }

        #endregion

        #region manage your expense
        public IActionResult EditExpense(int Id, DateTime date)
        {
            return ViewComponent("ExpenseWidget", new { expenseId = Id, date = date });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveExpense(ExpenseViewModel expense, string employeeName)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(employeeName))
                    expense.EmployeeName = CurrentEmployee();
                var data = _mapper.Map<Expense>(expense);
                _timesheetRepository.AddorUpdateExpense(data);
            }
            return ViewComponent("TimesheetWidget", new { userName = expense.EmployeeName, dateTime = expense.Date });
        }

        [HttpPost]
        public IActionResult DeleteExpense(int Id, DateTime date, string employeeName)
        {
            _timesheetRepository.RemoveExpense(Id);
            return ViewComponent("TimesheetWidget", new { userName = string.IsNullOrEmpty(employeeName) ? CurrentEmployee() : employeeName, dateTime = date });
        }
        #endregion

        #region manage your mileage
        public IActionResult EditMileage(int Id, DateTime date)
        {
            return ViewComponent("MileageWidget", new { mileageId = Id, date = date });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveMileage(MileageViewModel mileage, string employeeName)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(employeeName))
                    mileage.EmployeeName = CurrentEmployee();
                var data = _mapper.Map<Mileage>(mileage);
                _timesheetRepository.AddorUpdateMileage(data);
            }
            return ViewComponent("TimesheetWidget", new { userName = mileage.EmployeeName, dateTime = mileage.Date });
        }

        [HttpPost]
        public IActionResult DeleteMileage(int Id, DateTime date, string employeeName)
        {
            _timesheetRepository.RemoveMileage(Id);
            return ViewComponent("TimesheetWidget", new { userName = string.IsNullOrEmpty(employeeName) ? CurrentEmployee() : employeeName, dateTime = date });
        }
        #endregion
    }
}
