using AutoMapper;
using sbpc.Timesheet.Data.Entity;
using sbpc.Timesheet.Models;
using sbpc.Timesheet.Models.AccountViewModels;
using sbpc.Timesheet.Models.AdminViewModels;

namespace sbpc.Timesheet.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>(MemberList.None).ReverseMap();
            CreateMap<Job, JobViewModel>(MemberList.None).ReverseMap();
            CreateMap<Expense, ExpenseViewModel>(MemberList.None).ReverseMap();
            CreateMap<Hour, HourViewModel>(MemberList.None).ReverseMap();
            CreateMap<Mileage, MileageViewModel>(MemberList.None).ReverseMap();
        }
    }
}
