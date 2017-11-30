using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using sbpc.Timesheet.Models;

namespace sbpc.Timesheet.Controllers
{
    public class TimesheetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
