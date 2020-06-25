using System.Linq;
using AutoMapper;
using MyExpenses.Models;

namespace MyExpenses
{
    public class GroupResolver : ITypeConverter<GroupModel, GroupGetFullModel>
    {
        public GroupGetFullModel Convert(
            GroupModel source,
            GroupGetFullModel destination,
            ResolutionContext context) =>
                new GroupGetFullModel
                {
                    Id = source.Id,
                    Name = source.Name,
                    Users = source.GroupUser.Select(gp => gp.User).ToList()
                };
    }

    public class MyExpensesProfile : Profile
    {
        public MyExpensesProfile()
        {
            CreateMap<UserModel, UserModel>().ReverseMap();

            CreateMap<GroupGetModel, GroupModel>().ReverseMap();
            CreateMap<GroupModel, GroupGetFullModel>().ConvertUsing(new GroupResolver());
        }
    }
}
