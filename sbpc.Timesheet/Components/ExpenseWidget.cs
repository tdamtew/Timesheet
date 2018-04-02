using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using sbpc.Timesheet.Data;
using sbpc.Timesheet.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Components
{
    public class ExpenseWidget : ViewComponent
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;
        public ExpenseWidget(ITimesheetRepository timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(DateTime date, int expenseId = 0)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var jobs = _timesheetRepository.GetAllJobs().Where(x => x.Active);
            ViewBag.jobList = jobs == null ? null : jobs.ToList();
            if (expenseId == 0) return View(new ExpenseViewModel { Date = date == null ? DateTime.Now : date });
            var data = _timesheetRepository.GetExpense(expenseId);
            return View(_mapper.Map<ExpenseViewModel>(data));
        }
    }
}
