using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using sbpc.Timesheet.Data;
using sbpc.Timesheet.Helpers;
using sbpc.Timesheet.Models.AccountViewModels;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Components
{
    public class EmployeeWidget : ViewComponent
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;
        public EmployeeWidget(ITimesheetRepository timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(string userId = "")
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (string.IsNullOrEmpty(userId)) return View(new UserViewModel { IsEnabled = true, Role = Constants.Role.Employee });
            var data = _timesheetRepository.GetUser(userId);
            if (data == null) return View( new UserViewModel { IsEnabled = true, Role = Constants.Role.Employee });
            return View(_mapper.Map<UserViewModel>(data));
        }
    }
}
