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

            // Domain <> Model
            CreateMap<UserDomain, UserModel>().ReverseMap();
        }
    }
}
