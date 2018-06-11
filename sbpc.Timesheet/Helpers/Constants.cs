﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace sbpc.Timesheet.Helpers
{
    public static class Constants
    {
        public static double CostPerMile = 0.56;
        public static double OverTimeRate = 1.5;
        public static class Method
        {
            public static string Personal = "Personal";
            public static string Company = "Company";
        }
        public static class Role
        {
            public static string Employee = "Employee";
            public static string TimesheetAdmin = "TimesheetAdmin";
            public static string MasterAdmin = "MasterAdmin";
        }
        public static class PItem
        {
            public static string PaidTimeOff = "Paid Time Off";
            public static string Hourly = "Hourly";
            public static string RegularOT = "Hourly OT";
            public static string SBP = "SBP";
        }
        public static class Category
        {
            public static string Miscellaneous = "Miscellaneous";
            public static string Breakfast = "Breakfast";
            public static string Lunch = "Lunch";
            public static string Dinner = "Dinner";
            public static string Snacks = "Snacks";
            public static string Flight = "Flight";
            public static string Baggage = "Baggage";
            public static string Hotel = "Hotel";
            public static string RentalCar = "Rental Car";
            public static string Tolls = "Tolls";
            public static string Fuel = "Fuel";
        }

        public static IEnumerable<SelectListItem> Methods => typeof(Method).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => new SelectListItem { Text = x.GetValue(null).ToString(), Value = x.GetValue(null).ToString() });
        public static IEnumerable<SelectListItem> Categories => typeof(Category).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => new SelectListItem { Text = x.GetValue(null).ToString(), Value = x.GetValue(null).ToString() });
        public static IEnumerable<SelectListItem> Roles => typeof(Role).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => new SelectListItem { Text = x.GetValue(null).ToString(), Value = x.GetValue(null).ToString() });
    }
}
