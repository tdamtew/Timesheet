using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using sbpc.Timesheet.Data;
using sbpc.Timesheet.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Components
{
    public class HourWidget : ViewComponent
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;
        public HourWidget(ITimesheetRepository timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(DateTime date, int hourId = 0)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var jobs = _timesheetRepository.GetAllJobs();
            ViewBag.jobList = jobs == null ? null : jobs.ToList();
            if (hourId == 0) return View(new HourViewModel { Date = DateTime.Now });
            var data = _timesheetRepository.GetHour(hourId);
            return View(_mapper.Map<HourViewModel>(data));
        }
    }
}
