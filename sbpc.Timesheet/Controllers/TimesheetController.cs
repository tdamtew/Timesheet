using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using sbpc.Timesheet.Models;
using sbpc.Timesheet.Data;
using System;
using AutoMapper;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace sbpc.Timesheet.Controllers
{
    [Authorize]
    public class TimesheetController : Controller
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;

        public TimesheetController(ITimesheetRepository timesheetRepository,
            IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            //First default page.
            return View(GetTimesheet(DateTime.Now, 0));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public TimesheetViewModel GetTimesheet(DateTime date, int JobId)
        {
            var startOfWeek = DateTime.Today.AddDays((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)DateTime.Today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);
            var TimeLog = _timesheetRepository.GetTimesheet(startOfWeek, endOfWeek, User.Identity.Name, JobId);
            if (TimeLog == null) return null;
            return new TimesheetViewModel
            {
                StartDate = startOfWeek,
                EndDate = endOfWeek,
                Expenses = _mapper.Map<IEnumerable<ExpenseViewModel>>(TimeLog.Expenses),
                Hours = _mapper.Map<IEnumerable<HourViewModel>>(TimeLog.Hours),
                Mileages = _mapper.Map<IEnumerable<MileageViewModel>>(TimeLog.Mileages)
            };

        }
    }
}
