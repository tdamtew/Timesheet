using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using sbpc.Timesheet.Data;
using sbpc.Timesheet.Models.AdminViewModels;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Components
{
    public class JobWidget : ViewComponent
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;
        public JobWidget(ITimesheetRepository timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(int jobId = 0)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (jobId == 0) return View(new JobViewModel { Active = true});
            var data = _timesheetRepository.GetJob(jobId);
            if (data == null) return View( new JobViewModel { Active = true });
            return View(_mapper.Map<JobViewModel>(data));
        }
    }
}
