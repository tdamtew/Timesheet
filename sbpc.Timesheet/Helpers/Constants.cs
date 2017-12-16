using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace sbpc.Timesheet.Helpers
{
    public static class Constants
    {
        public static class Method
        {
            public static string Personal = "Personal";
            public static string Company = "Company";
        }
        public static class Item
        {
            public static string Billable = "Billable";
            public static string NonBillable = "Non-Billable";
            public static string Overtime = "Overtime";
            public static string Travel = "Travel";
            public static string Driver = "Driver";
            public static string DriverOT = "Driver OT";
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
        public static IEnumerable<SelectListItem> Items => typeof(Item).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => new SelectListItem { Text = x.GetValue(null).ToString(), Value = x.GetValue(null).ToString() });
        public static IEnumerable<SelectListItem> Categories => typeof(Category).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => new SelectListItem { Text = x.GetValue(null).ToString(), Value = x.GetValue(null).ToString() });
    }
}
