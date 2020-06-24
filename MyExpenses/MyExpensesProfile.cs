using AutoMapper;
using MyExpenses.Models;

namespace MyExpenses
{
    public class MyExpensesProfile : Profile
    {
        public MyExpensesProfile()
        {
            CreateMap<UserModel, UserModelToAdd>().ReverseMap();
            CreateMap<UserModel, UserModel>().ReverseMap();
        }
    }
}
