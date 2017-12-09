using Microsoft.AspNetCore.Authorization;
using sbpc.Timesheet.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Helpers
{
    public class AdminRequirement : AuthorizationHandler<AdminRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            return Task.CompletedTask;
        }
    }
}
