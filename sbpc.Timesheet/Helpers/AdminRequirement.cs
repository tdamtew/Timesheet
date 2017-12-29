using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Helpers
{
    public class AdminRequirement : AuthorizationHandler<AdminRequirement>, IAuthorizationRequirement
    {
        private readonly string _userId;
        public AdminRequirement(string userId)
        {
            _userId = userId;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            if (string.Compare(context.User.Identity.Name, _userId, true) == 0)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public class TimesheetAdminRequirement : AuthorizationHandler<TimesheetAdminRequirement>, IAuthorizationRequirement
    {
        private readonly string _userId;
        public TimesheetAdminRequirement(string userId)
        {
            _userId = userId;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TimesheetAdminRequirement requirement)
        {
            if (string.Compare(context.User.Identity.Name, _userId, true) == 0)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
