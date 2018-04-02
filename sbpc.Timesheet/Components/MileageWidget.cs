using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using sbpc.Timesheet.Data;
using sbpc.Timesheet.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Components
{
    public class MileageWidget : ViewComponent
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;
        public MileageWidget(ITimesheetRepository timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(DateTime date, int mileageId = 0)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var jobs = _timesheetRepository.GetAllJobs().Where(x => x.Active);
            ViewBag.jobList = jobs?.ToList();
            if (mileageId == 0) return View(new MileageViewModel { Date = date == null ? DateTime.Now : date });
            var data = _timesheetRepository.GetMileage(mileageId);
            return View(_mapper.Map<MileageViewModel>(data));
        }
    }
}
