using Microsoft.AspNetCore.Identity;
using sbpc.Timesheet.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace sbpc.Timesheet.Helpers
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser>
    {
        public ApplicationSignInManager(UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ApplicationUser>> logger, IAuthenticationSchemeProvider schemes)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
        }

        public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var user = UserManager.FindByEmailAsync(userName).Result;

            if (user == null)
            {
                return Task.FromResult(SignInResult.Failed);
            }

            if (!user.IsEnabled)
            {
                return Task.FromResult(SignInResult.LockedOut);
            }
            var userClaims = UserManager.GetClaimsAsync(user).Result;
            var removeClaim = UserManager.RemoveClaimsAsync(user, userClaims.Where(x => x.Type == "Role")).Result;
            var addClaim = UserManager.AddClaimAsync(user, new System.Security.Claims.Claim("Role", user.Role)).Result;
            return base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        }
    }
}
