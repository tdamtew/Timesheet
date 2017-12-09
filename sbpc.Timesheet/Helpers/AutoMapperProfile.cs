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
            CreateMap<JobViewModel, Job>(MemberList.None)
                .ForMember(d => d.Id, opt => opt.UseDestinationValue()).ReverseMap();
            CreateMap<ExpenseViewModel, Expense>(MemberList.None)
                .ForMember(d => d.Id, opt => opt.UseDestinationValue()).ReverseMap();
            CreateMap<HourViewModel, Hour>(MemberList.None)
                .ForMember(d => d.Id, opt => opt.UseDestinationValue()).ReverseMap();
            CreateMap<MileageViewModel, Mileage>(MemberList.None)
                .ForMember(d => d.Id, opt => opt.UseDestinationValue()).ReverseMap();
        }
    }
}
