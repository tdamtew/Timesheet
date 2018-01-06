using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sbpc.Timesheet.Data.Entity;
using sbpc.Timesheet.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using sbpc.Timesheet.Services;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using sbpc.Timesheet.Models.AdminViewModels;
using sbpc.Timesheet.Data;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace sbpc.Timesheet.Controllers
{
    [Authorize(policy: "AdminRole")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager  <ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;

        public AdminController(UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ITimesheetRepository timesheetRepository,
          IMapper mapper,
          ILogger<ManageController> logger,
          UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _mapper = mapper;
            _timesheetRepository = timesheetRepository;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var jobsData = _timesheetRepository.GetAllJobs();
            var employeesData = _timesheetRepository.GetAllUsers();

            if (jobsData != null)
            {
                var jobList = jobsData.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).ToList();
                jobList.Add(new SelectListItem { Value = "", Text = "All", Selected = true });
                ViewBag.jobList = jobList;
            }
            if (employeesData != null)
            {
                var employeeList = employeesData.Select(x => new SelectListItem { Value = $"{x.FirstName} {x.LastName}", Text = $"{x.FirstName} {x.LastName}" }).ToList();
                employeeList.Add(new SelectListItem { Value = "", Text = "All", Selected = true });
                ViewBag.employeeList = employeeList;
            }
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ViewBag.startDate = startDate;
            ViewBag.endDate = startDate.AddMonths(1).AddDays(-1);
            return View();
        }

        [HttpPost]
        public IActionResult Search(string userId, string jobName, DateTime startDate, DateTime endDate)
        {
            return ViewComponent("TimesheetAdminWidget", new { startDate = startDate, endDate = endDate, userId = userId, jobName = jobName });
        }

        #region manage employees

        [HttpGet]
        [Route("Admin/Users")]
        public IActionResult Users()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //check if the user exists
                    var user = _mapper.Map<ApplicationUser>(model);
                    var data = await _userManager.FindByNameAsync(model.Email);
                    if (data == null)
                    {
                        user.UserName = model.Email;
                        var tempPassword = "Welcome321!";
                        var createUserResult = await _userManager.CreateAsync(user, tempPassword);
                        if (createUserResult.Succeeded)
                        {
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                            await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
                            _logger.LogInformation($"user {model.Email} has been created.");
                        }
                    }
                    else
                    {
                        _timesheetRepository.UpdateUser(user);
                        _logger.LogInformation($"user {model.Email} has been updated successfully!");
                    }
                    return ViewComponent("EmployeesWidget");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error while creating new employee {model.Email} with message {ex.Message}");
                    return StatusCode(500);
                }
            }
            return ViewComponent("EmployeesWidget");
        }

        [HttpGet]
        [Route("Admin/EditUser")]
        public IActionResult EditUser(string userId)
        {
            return ViewComponent("EmployeeWidget", new { userId = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userId);
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return ViewComponent("EmployeesWidget");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred while removing employee {userId} with message {ex.Message}");
                return StatusCode(500);
            }
        }

        #endregion

        #region manage jobs
        [HttpGet]
        public IActionResult Jobs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveJob(JobViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _timesheetRepository.AddorUpdateJob(_mapper.Map<Job>(model));
                    _logger.LogInformation($"{model.Name} has been added/updated successfully!");
                    return ViewComponent("JobsWidget");
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(string.Format("Exception occurred while creating Job {0} with message {1}", model.Name, ex.Message));
                    return StatusCode(500);
                }
            }
            return ViewComponent("JobsWidget");
        }

        [HttpGet]
        [Route("Admin/EditJob")]
        public IActionResult EditJob(int jobId)
        {
            return ViewComponent("JobWidget", new { jobId = jobId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteJob(int jobId)
        {
            try
            {
                var removedJob = _timesheetRepository.RemoveJob(jobId);
                if (removedJob > 0)
                {
                    return ViewComponent("JobsWidget");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred while removing job {jobId} with message {ex.Message}");
                return StatusCode(500);
            }
        }
        #endregion
    }
}