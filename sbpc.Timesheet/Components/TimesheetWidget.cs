using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using sbpc.Timesheet.Data;
using sbpc.Timesheet.Data.Model;
using sbpc.Timesheet.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Components
{
    public class TimesheetWidget : ViewComponent
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;

        public TimesheetWidget(ITimesheetRepository timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(string userName, DateTime dateTime)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var data = _timesheetRepository.GetUserTimesheet(dateTime, userName);
            if (data == null) return View(new TimesheetViewModel { date = dateTime });
            return View(new TimesheetViewModel
            {
                date = dateTime,
                Expenses = _mapper.Map<IEnumerable<ExpenseViewModel>>(data.Expenses),
                Hours = _mapper.Map<IEnumerable<HourViewModel>>(data.Hours),
                Mileages = _mapper.Map<IEnumerable<MileageViewModel>>(data.Mileages)
            });
        }

    }
}
