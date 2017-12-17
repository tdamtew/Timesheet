using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Components
{
    public class NavWidget : ViewComponent
    {

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(string userName)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var navpage = new List<NavPage>();
            if (String.CompareOrdinal(userName, "tadesse.eshetu@gmail.com") == 0)
            {
                return View(AdminPages());
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
                new NavPage { root = "Timesheet",
                    links = new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                    { new KeyValuePair<string, string>("Timesheet Admin",Url.Action("Index","Admin")) }) },
                new NavPage { root = "Employees",
                    links = new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                    { new KeyValuePair<string, string>("All Employees",Url.Action("Users","Admin")),
                            new KeyValuePair<string, string>("Add Employee", Url.Action("AddUser","Admin")) }) },
                new NavPage { root = "Jobs",
                    links = new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                    { new KeyValuePair<string, string>("All Jobs",Url.Action("Jobs","Admin")),
                            new KeyValuePair<string, string>("Add Job", Url.Action("AddJob","Admin")) })
                },
                new NavPage { root = "Account",
                    links = new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                    { new KeyValuePair<string, string>("Profile",Url.Action("Index","Manage")),
                    new KeyValuePair<string, string>("Password", Url.Action("ChangePassword","Manage")) })

                }

            };
        }

        private IEnumerable<NavPage> UserPages()
        {
            return new List<NavPage>
            {
                new NavPage { root = "Timesheet",
                    links = new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                    { new KeyValuePair<string, string>("Timesheet", Url.Action("Index","Timesheet")) }) },
                new NavPage { root = "Account",
                    links = new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                    { new KeyValuePair<string, string>("Profile",Url.Action("Index","Manage")),
                            new KeyValuePair<string, string>("Password", Url.Action("ChangePassword","Manage")) }) }
            };
        }
    }
}
