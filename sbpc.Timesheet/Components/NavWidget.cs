using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Components
{
    public class NavWidget : ViewComponent
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal User)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var navpage = new List<NavPage>();
            if (User.HasClaim("Role", "MasterAdmin"))
            {
                return View(AdminPages());
            }
            else if(User.HasClaim("Role", "TimesheetAdmin"))
            {
                return View(TimesheetAdminPages());
            }
            return View(UserPages());
        }

        public class NavPage
        {
            public string root { get; set; }
            public Dictionary<string, string> links { get; set; }
        }

        private IEnumerable<NavPage> AdminPages()
        {
            return new List<NavPage>
            {
                new NavPage {
                        root = "Home",
                        links = new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("Timesheet",Url.Action("Index","Timesheet"))
                        })
                    },
                new NavPage {
                        root = "Admin",
                        links = new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("Timesheets",Url.Action("Index","Admin")),
                            new KeyValuePair<string, string>("Employees",Url.Action("Users","Admin")),
                            new KeyValuePair<string, string>("Jobs",Url.Action("Jobs","Admin"))
                        })
                    },
                new NavPage {
                    root = "Account",
                    links = new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Change password", Url.Action("ChangePassword","Manage"))
                    })
                }
            };
        }
        private IEnumerable<NavPage> TimesheetAdminPages()
        {
            return new List<NavPage>
            {
                new NavPage {
                        root = "Home",
                        links = new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("Timesheet",Url.Action("Index","Timesheet"))
                        })
                    },
                new NavPage {
                    root = "Account",
                    links = new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Change password", Url.Action("ChangePassword","Manage"))
                    })
                }
            };
        }

        private IEnumerable<NavPage> UserPages()
        {
            return new List<NavPage>
            {
                new NavPage {
                        root = "Home",
                        links = new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("My Timesheet",Url.Action("Index","Timesheet"))
                        })
                    },
                new NavPage {
                    root = "Account",
                    links = new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Change password", Url.Action("ChangePassword","Manage"))
                    })
                }
            };
        }
    }
}
