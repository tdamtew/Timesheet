using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace sbpc.Timesheet.Helpers
{
    public static class Constants
    {
        public static double CostPerMile = 0.56;
        public static class Method
        {
            public static string Personal = "Personal";
            public static string Company = "Company";
        }
        public static class PItem
        {
            public static string Hourly = "Hourly";
            public static string RegularOT = "Hourly OT";
            public static string SBP = "SBP";
        }
        public static class Item
        {
            public static string LeadEngineer = "Lead Engineer";
            public static string Engineer = "Engineer";
            public static string CATP = "CATP Tech";
            public static string Driver = "Driver";
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
