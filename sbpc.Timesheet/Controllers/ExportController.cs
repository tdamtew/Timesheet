﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using sbpc.Timesheet.Data;
using System;
using System.Linq;
using sbpc.Timesheet.Models;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using static sbpc.Timesheet.Helpers.Constants;

namespace sbpc.Timesheet.Controllers
{
    [Authorize(policy: "AdminRole")]
    public class ExportController : Controller
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IConfiguration _configuration;

        public ExportController(ITimesheetRepository timesheetRepository, IConfiguration configuration)
        {
            _timesheetRepository = timesheetRepository;
            _configuration = configuration;
        }

        public IActionResult Index(DateTime startDate, DateTime endDate, bool exportAll = false)
        {
            var data = _timesheetRepository.GetHours(startDate, endDate);
            if (!exportAll && data != null && data.Any())
            {
                data = data.Where(x => !x.IsExported);
            }
            if (data == null) return View();
            var model = data.GroupBy(x => new { x.EmployeeName, x.JobName }).Select(x => new ItemViewModel { Employee = x.Key.EmployeeName, Job = x.Key.JobName }).ToList();
            ViewBag.startDate = startDate;
            ViewBag.exportAll = exportAll;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult Export(List<ItemViewModel> items, DateTime date, bool exportAll = false)
        {
            var data = GetDataToExport(items, date, exportAll);
            var buffer = Encoding.ASCII.GetBytes(data);
            return File(buffer, "text/iif", $"timesheet_{date.ToString("MMddyyyy")}.iff");
        }

        private string GetDataToExport(List<ItemViewModel> model, DateTime date, bool exportAll)
        {
            var _dataToExport = new StringBuilder();

            //define date range.
            var startOfMonth = new DateTime(date.Year, date.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            //create iif header.
            _dataToExport.AppendLine($"!TIMERHDR\tVER\tREL\tCOMPANYNAME\tIMPORTEDBEFORE\tFROMTIMER\tCOMPANYCREATETIME");
            _dataToExport.AppendLine($"TIMERHDR\t8\t0\tSBP Consulting, LLC\tN\tN\t1243092298");
            _dataToExport.AppendLine($"!HDR\tPROD\tVER\tREL\tIIFVER\tDATE\tTIME\tACCNTNT\tACCNTNTSPLITTIME");
            _dataToExport.AppendLine($"HDR\tQuickBooks Pro for Windows\tVersion 6.0D\tRelease R6P\t1\t3/22/2012\t1332451825\tN\t0");
            _dataToExport.AppendLine($"!TIMEACT\tDATE\tJOB\tEMP\tITEM\tPITEM\tDURATION\tPROJ\tNOTE\tBILLINGSTATUS");

            //create iif data.
            var employees = model.Select(x => x.Employee).Distinct().ToList();
            foreach (var emp in employees)
            {
                var d = startOfMonth;
                while (d <= endOfMonth)
                {
                    var data = GetWeeklyData(emp, d, model.Where(x => x.Employee == emp), exportAll);
                    if (data == null || !data.Any())
                    {
                        d = d.AddDays(7);
                        continue;
                    }
                    foreach (var t in data)
                    {
                        _dataToExport.AppendLine($"TIMEACT\t{t.Date.ToShortDateString()}\t{t.Job}\t{t.Employee}\t{t.Item}\t{t.PayableItem}\t{t.Duration}\t{t.Project}\t{t.Note}\t{t.BillingStatus}");
                    }
                    d = d.AddDays(7);
                }
            }
            return _dataToExport.ToString();
        }

        private List<ExportViewModel> GetWeeklyData(string employee, DateTime dateOfWeek, IEnumerable<ItemViewModel> items, bool exportAll)
        {
            var startOfWeek = dateOfWeek.AddDays((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)dateOfWeek.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);
            var data = _timesheetRepository.GetHours(startOfWeek, endOfWeek, employee);
            if (!exportAll && data != null && data.Any())
            {
                data = data.Where(x => !x.IsExported);
            }
            if (data == null || !data.Any()) return null;
            var exportView = new List<ExportViewModel>();
            foreach (var d in data.OrderBy(a => a.Date))
            {
                exportView.Add(new ExportViewModel
                {
                    Date = d.Date,
                    Job = d.JobName,
                    Employee = d.EmployeeName,
                    Duration = d.Hours - d.OTHours,
                    Item = items.Where(x => x.Job == d.JobName).Select(x => x.Type).FirstOrDefault(),
                    PayableItem = d.Billable ? PItem.Hourly : PItem.SBP,
                    BillingStatus = d.Billable ? 1 : 0,
                    Note = d.Note
                });
                if (d.OTHours > 0)
                {
                    exportView.Add(new ExportViewModel
                    {
                        Date = d.Date,
                        Job = d.JobName,
                        Employee = d.EmployeeName,
                        Duration = d.OTHours,
                        Item = items.Where(x => x.Job == d.JobName).Select(x => x.Type).FirstOrDefault(),
                        PayableItem = PItem.RegularOT,
                        BillingStatus = d.Billable ? 1 : 0,
                        Note = d.Note
                    });
                }
                _timesheetRepository.UpdateExportFlag(d);
                continue;
            }
            return exportView;
        }
    }
}