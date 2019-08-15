using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;

namespace lfmachadodasilva.MyExpenses.Api
{
    public class MyExpensesProfile : Profile
    {
        public MyExpensesProfile()
        {
            CreateMap<GroupModel, GroupModel>();
            CreateMap<GroupModel, GroupDto>().ReverseMap();

            CreateMap<ExpenseModel, ExpenseModel>();
            CreateMap<ExpenseModel, ExpenseDto>().ReverseMap();
            CreateMap<ExpenseModel, ExpenseWithValuesDto>()
                .ForMember(
                    dst => dst.LabelName,
                    opt => opt.MapFrom(src => src.Label.Name))
                .ReverseMap();

            CreateMap<LabelModel, LabelModel>();
            CreateMap<LabelModel, LabelDto>().ReverseMap();
            CreateMap<LabelModel, LabelWithValuesDto>().ReverseMap();

            CreateMap<UserModel, UserModel>();
            CreateMap<UserModel, UserDto>().ReverseMap();
        }
    }
}
