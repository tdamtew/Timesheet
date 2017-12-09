using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace sbpc.Timesheet.Views.Admins
{
    public static class AdminNavPages
    {
        public static string ActivePageKey => "ActivePage";

        public static string Timesheet => "Index";

        public static string Employees => "Users";

        public static string AddEmployee => "AddUser";

        public static string EditEmployee => "EditUser";

        public static string Jobs => "Jobs";

        public static string AddJob => "AddJob";

        public static string EditJob => "EditJob";

        public static string ChangePassword => "ChangePassword";


        public static string TimesheetNavClass(ViewContext viewContext) => PageNavClass(viewContext, Timesheet);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string EmployeesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Employees);

        public static string AddEmployeeNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddEmployee);

        public static string EditEmployeeNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditEmployee);

        public static string JobsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Jobs);

        public static string AddJobNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddJob);

        public static string EditJobNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditJob);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}
