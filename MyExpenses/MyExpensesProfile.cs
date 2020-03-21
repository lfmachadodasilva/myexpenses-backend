using AutoMapper;
using MyExpenses.Models;
using MyExpenses.Models.Domains;
using MyExpenses.Models.Dtos;

namespace MyExpenses
{
    public class MyExpensesProfile : Profile
    {
        public MyExpensesProfile()
        {
            // Dto <> Domain
            CreateMap<UserDto, UserDomain>().ReverseMap();
            CreateMap<GroupDto, GroupDomain>().ReverseMap();
            CreateMap<GroupDetailsDto, GroupDetailsDomain>().ReverseMap();

            // Domain <> Model
            CreateMap<UserDomain, UserModel>().ReverseMap();
            CreateMap<GroupDomain, GroupModel>().ReverseMap();
        }
    }
}
