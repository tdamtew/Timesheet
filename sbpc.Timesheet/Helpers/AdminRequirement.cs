using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Helpers
{
    public class AdminRequirement : AuthorizationHandler<AdminRequirement>, IAuthorizationRequirement
    {
        private readonly string[] _users;
        public AdminRequirement(string [] users)
        {
            _users = users;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            if(_users.Contains(context.User.Identity.Name))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public class TimesheetAdminRequirement : AuthorizationHandler<TimesheetAdminRequirement>, IAuthorizationRequirement
    {
        private readonly string[] _users;
        public TimesheetAdminRequirement(string[] users)
        {
            _users = users;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TimesheetAdminRequirement requirement)
        {
            if (_users.Contains(context.User.Identity.Name))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
