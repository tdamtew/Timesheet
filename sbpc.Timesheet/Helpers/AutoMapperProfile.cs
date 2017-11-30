using AutoMapper;
using sbpc.Timesheet.Data.Entity;
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
        }
    }
}
