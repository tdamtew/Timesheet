using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Helpers
{
    public class AdminRequirement : AuthorizationHandler<AdminRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            if (context.User.HasClaim("Role", Constants.Role.MasterAdmin))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public class TimesheetAdminRequirement : AuthorizationHandler<TimesheetAdminRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TimesheetAdminRequirement requirement)
        {
            if (context.User.HasClaim("Role", Constants.Role.TimesheetAdmin) || context.User.HasClaim("Role", Constants.Role.MasterAdmin))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
