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

namespace sbpc.Timesheet.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ITimesheetRepository _tsrepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;

        public AdminController(UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ITimesheetRepository tsrepository,
          IMapper mapper,
          ILogger<ManageController> logger,
          UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _mapper = mapper;
            _tsrepository = tsrepository;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        [HttpGet]
        [Route("Admin/Users")]
        public IActionResult Users()
        {
            var appUsers = _tsrepository.GetAllUsers();
            if (!appUsers.Any()) return View();
            var users = _mapper.Map<IEnumerable<UserViewModel>>(appUsers);
            return View(users);
        }

        [HttpGet]
        [Route("Admin/AddUser")]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Jobs()
        {
            var jobs = _tsrepository.GetAllJobs();
            if (!jobs.Any()) return View();
            var jobViewModel = _mapper.Map<IEnumerable<JobViewModel>>(jobs);
            return View(jobViewModel);
        }

        [HttpGet]
        [Route("Admin/AddJob")]
        public IActionResult AddJob()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddJob(JobViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _tsrepository.AddJob(_mapper.Map<Job>(model));
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
                    _logger.LogInformation(string.Format("user {0} has been created", model.Email));
                    return View("Success");
                }
                AddErrors(createUserResult);
            }
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}