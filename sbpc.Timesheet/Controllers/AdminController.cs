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
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;

namespace sbpc.Timesheet.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
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
            return View();
        }

        [HttpGet]
        [Route("Admin/Users")]
        public IActionResult Users()
        {
            var appUsers = _timesheetRepository.GetAllUsers();
            if (!appUsers.Any()) return View();
            var users = _mapper.Map<IEnumerable<UserViewModel>>(appUsers);
            return View(users);
        }

        [HttpGet]
        [Route("Admin/AddUser")]
        public IActionResult AddUser()
        {
            return View(new UserViewModel { });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<ApplicationUser>(model);
                user.UserName = model.Email;
                var tempPassword = "Welcome321!";
                var createUserResult = await _userManager.CreateAsync(user, tempPassword);
                if (createUserResult.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
                    _logger.LogInformation($"user {model.Email} has been created.");
                    model.StatusMessage = "Employee Information has been created successfully!";
                    return View(model);
                }

                model.StatusMessage = $"Error while creating new employee {model.Email}.";
                _logger.LogError($"Error while creating new employee {model.Email}");
            }
            return View(model);
        }

        [HttpGet]
        [Route("Admin/EditUser")]
        public IActionResult EditUser(string userId)
        {
            var user = _timesheetRepository.GetUser(userId);
            if (user == null) return View();
            var model = _mapper.Map<UserViewModel>(user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _mapper.Map<ApplicationUser>(model);
                    _timesheetRepository.UpdateUser(user);
                    model.StatusMessage = "Employee Information has been updated successfully!";
                    return View(model);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError($"Exception occurred while updating employee {model.Email} with message {ex.Message}");
                    model.StatusMessage = "Error occurred while updating Employee's Profile. Please try again.";
                    return View(model);
                }
            }
            return View();
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
                {
                    return StatusCode(200);
                }
                return NotFound();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception occurred while removing employee {userId} with message {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult Jobs()
        {
            var jobs = _timesheetRepository.GetAllJobs();
            if (!jobs.Any()) return View();
            var jobViewModel = _mapper.Map<IEnumerable<JobViewModel>>(jobs);
            return View(jobViewModel);
        }

        [HttpGet]
        [Route("Admin/AddJob")]
        public IActionResult AddJob()
        {
            return View(new JobViewModel { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddJob(JobViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _timesheetRepository.AddJob(_mapper.Map<Job>(model));
                    model.StatusMessage = "Job has been created successfully!";
                    return View(model);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(string.Format("Exception occurred while creating Job {0} with message {1}", model.Name, ex.Message));
                    ModelState.AddModelError(string.Empty, string.Format("Error occurred while creating job {0}", model.Name));
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Route("Admin/EditJob")]
        public IActionResult EditJob(int id)
        {
            var job = _timesheetRepository.GetJob(id);
            if (job == null) return View();
            var model = _mapper.Map<JobViewModel>(job);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditJob(JobViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var job = _mapper.Map<Job>(model);
                    _timesheetRepository.UpdateJob(job);
                    model.StatusMessage = "Job Information has been updated successfully!";
                    return View(model);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError($"Exception occurred while updating job {model.Name} with message {ex.Message}");
                    model.StatusMessage = "Error occurred while updating job. Please try again.";
                    return View(model);
                }
            }
            return View();
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
                    return StatusCode(200);
                }
                return NotFound();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception occurred while removing job {jobId} with message {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}