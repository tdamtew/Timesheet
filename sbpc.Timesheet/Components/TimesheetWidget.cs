﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using sbpc.Timesheet.Data;
using sbpc.Timesheet.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            //get extended month data
            var startOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var startWeekOfStartOfMonth = startOfMonth.AddDays((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)startOfMonth.DayOfWeek);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var startWeekOfEndOfMonth = endOfMonth.AddDays((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)endOfMonth.DayOfWeek);
            var endWeekOfEndOfMonth = startWeekOfEndOfMonth.AddDays(6);

            var data = _timesheetRepository.GetTimesheet(startWeekOfStartOfMonth, endWeekOfEndOfMonth, userName);
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
