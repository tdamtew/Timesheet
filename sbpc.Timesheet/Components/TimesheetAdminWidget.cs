using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using sbpc.Timesheet.Data;
using sbpc.Timesheet.Data.Model;
using sbpc.Timesheet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Components
{
    public class TimesheetAdminWidget : ViewComponent
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;

        public TimesheetAdminWidget(ITimesheetRepository timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(DateTime startDate, DateTime endDate, string userId, string jobName)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            var data = _timesheetRepository.GetTimesheet(startDate, endDate, userId, jobName);
            if (data == null) return View(new TimesheetViewModel { });
            return View(new TimesheetViewModel
            {
                Expenses = _mapper.Map<IEnumerable<ExpenseViewModel>>(data.Expenses),
                Hours = _mapper.Map<IEnumerable<HourViewModel>>(data.Hours),
                Mileages = _mapper.Map<IEnumerable<MileageViewModel>>(data.Mileages)
            });
        }

    }
}
