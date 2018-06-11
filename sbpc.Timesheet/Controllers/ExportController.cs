using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using sbpc.Timesheet.Data;
using System;
using System.Linq;
using sbpc.Timesheet.Models;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using static sbpc.Timesheet.Helpers.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using NPOI.XSSF.UserModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace sbpc.Timesheet.Controllers
{
    [Authorize(policy: "AdminRole")]
    public class ExportController : Controller
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ExportController(ITimesheetRepository timesheetRepository, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _timesheetRepository = timesheetRepository;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index(DateTime startDate, DateTime endDate, string userId, string jobName, bool exportAll = false)
        {
            var data = _timesheetRepository.GetHours(startDate, endDate);
            if (!string.IsNullOrEmpty(userId))
                data = data.Where(x => x.EmployeeName == userId);
            if (!string.IsNullOrEmpty(jobName))
                data = data.Where(x => x.JobName == jobName);
            if (!exportAll && data != null && data.Any())
            {
                data = data.Where(x => !x.IsExported);
            }
            if (data == null) return View();
            var model = data.GroupBy(x => new { x.EmployeeName, x.JobName }).Select(x => new ItemViewModel { Employee = x.Key.EmployeeName, Job = x.Key.JobName }).ToList();
            ViewBag.Items = _configuration.GetSection("Data:Items").Get<IEnumerable<string>>().Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            ViewBag.jobName = jobName;
            ViewBag.exportAll = exportAll;
            return View(model);
        }

        public async Task<IActionResult> Expense(DateTime startDate, DateTime endDate, string userId, string jobName, bool exportAll = false)
        {
            var data = GetExpenseData(startDate, endDate, userId, jobName, exportAll);

            string rootFolder = _hostingEnvironment.WebRootPath;
            var fileName = $"sbptimesheet_expense_report.xlsx";
            var file = new FileInfo(Path.Combine(rootFolder, fileName));
            var memory = new MemoryStream();

            using (var fs = new FileStream(Path.Combine(rootFolder, fileName), FileMode.Create, FileAccess.Write))
            {
                var workbook = new XSSFWorkbook();
                var excelSheet = workbook.CreateSheet("Expense");
                var headerfont = workbook.CreateFont();
                headerfont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                headerfont.Underline = NPOI.SS.UserModel.FontUnderlineType.Single;
                var cellStyle = workbook.CreateCellStyle();
                cellStyle.SetFont(headerfont);

                //create header rows.
                var row = excelSheet.CreateRow(0);
                var cell = row.CreateCell(0);
                cell.CellStyle = cellStyle;
                cell.SetCellValue("Date");

                cell = row.CreateCell(1);
                cell.CellStyle = cellStyle;
                cell.SetCellValue("Job");

                cell = row.CreateCell(2);
                cell.CellStyle = cellStyle;
                cell.SetCellValue("Employee");

                cell = row.CreateCell(3);
                cell.CellStyle = cellStyle;
                cell.SetCellValue("Category");

                cell = row.CreateCell(4);
                cell.CellStyle = cellStyle;
                cell.SetCellValue("Amount");

                cell = row.CreateCell(5);
                cell.CellStyle = cellStyle;
                cell.SetCellValue("Method");

                cell = row.CreateCell(6);
                cell.CellStyle = cellStyle;
                cell.SetCellValue("Note");

                //insert data.
                var rowCounter = 1;
                if (data != null && data.Any())
                {
                    foreach (var d in data)
                    {
                        row = excelSheet.CreateRow(rowCounter);
                        row.CreateCell(0).SetCellValue(d.Date.ToString("MM/dd/yyyy"));
                        row.CreateCell(1).SetCellValue(d.Job);
                        row.CreateCell(2).SetCellValue(d.Employee);
                        row.CreateCell(3).SetCellValue(d.Category);
                        row.CreateCell(4).SetCellValue(d.Amount.ToString("C"));
                        row.CreateCell(5).SetCellValue(d.Method);
                        row.CreateCell(6).SetCellValue(d.Note);
                        rowCounter++;
                    }
                }
                rowCounter = 0;
                workbook.Write(fs);
                fs.Close();
            }
            using (var stream = new FileStream(Path.Combine(rootFolder, fileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult Export(List<ItemViewModel> items, DateTime startDate, DateTime endDate, string jobName, bool exportAll = false)
        {
            var data = GetDataToExport(items, startDate, endDate, jobName, exportAll);
            var buffer = Encoding.ASCII.GetBytes(data);
            return File(buffer, "text/iif", $"timesheet_{startDate.ToString("MMddyyyy")}_{endDate.ToString("MMddyyyy")}.iif");
        }

        private string GetDataToExport(List<ItemViewModel> model, DateTime startDate, DateTime endDate, string jobName, bool exportAll)
        {
            var _dataToExport = new StringBuilder();

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

                var data = GetData(emp, startDate, endDate, jobName, model.Where(x => x.Employee == emp), exportAll);
                if (data == null || !data.Any())
                {
                    continue;
                }
                foreach (var t in data)
                {
                    _dataToExport.AppendLine($"TIMEACT\t{t.Date.ToShortDateString()}\t{t.Job}\t{t.Employee}\t{t.Item}\t{t.PayableItem}\t{t.Duration}\t{t.Project}\t{t.Note}\t{t.BillingStatus}");
                }
            }
            return _dataToExport.ToString();
        }

        private List<ExportViewModel> GetData(string employee, DateTime startDate, DateTime endDate, string jobName, IEnumerable<ItemViewModel> items, bool exportAll)
        {
            var data = _timesheetRepository.GetHours(startDate, endDate, employee);
            if (!string.IsNullOrEmpty(jobName))
                data = data.Where(x => x.JobName == jobName);
            if (!exportAll && data != null && data.Any())
            {
                data = data.Where(x => !x.IsExported);
            }
            if (data == null || !data.Any()) return null;
            var exportView = new List<ExportViewModel>();
            foreach (var d in data.OrderBy(a => a.Date))
            {
                if (d.Hours == 0 || (d.Hours - d.OTHours) > 0)
                {
                    exportView.Add(new ExportViewModel
                    {
                        Date = d.Date,
                        Job = d.JobName,
                        Employee = d.EmployeeName,
                        Duration = d.Hours - d.OTHours,
                        Item = items.Where(x => x.Job == d.JobName).Select(x => x.Type).FirstOrDefault(),
                        PayableItem = d.JobName == PItem.PaidTimeOff ? PItem.PaidTimeOff : d.Billable ? PItem.Hourly : PItem.SBP,
                        BillingStatus = d.Billable ? 1 : 0,
                        Note = d.Note
                    });
                }
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
        private List<ExportExpenseViewModel> GetExpenseData(DateTime startDate, DateTime endDate, string employee, string jobName, bool exportAll)
        {
            var data = _timesheetRepository.GetExpenses(startDate, endDate, employee);
            if (!string.IsNullOrEmpty(jobName))
                data = data.Where(x => x.JobName == jobName);
            if (!exportAll && data != null && data.Any())
            {
                data = data.Where(x => !x.IsExported);
            }
            if (data == null || !data.Any()) return null;
            var exportView = new List<ExportExpenseViewModel>();
            foreach (var d in data.OrderBy(a => a.Date))
            {
                exportView.Add(new ExportExpenseViewModel
                {
                    Date = d.Date,
                    Job = d.JobName,
                    Employee = d.EmployeeName,
                    Category = d.Category,
                    Amount = d.Amount,
                    Method = d.Method,
                    Note = d.Note
                });
                _timesheetRepository.UpdateExportFlag(d);
                continue;
            }
            return exportView;
        }
    }
}