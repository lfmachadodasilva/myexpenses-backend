using System.Linq;
using AutoMapper;
using MyExpenses.Models;

namespace MyExpenses
{
    public class GroupToFullResolver : ITypeConverter<GroupModel, GroupGetFullModel>
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

    public class GroupFromManagerResolver : ITypeConverter<GroupManageModel, GroupModel>
    {
        public GroupModel Convert(
            GroupManageModel source,
            GroupModel destination,
            ResolutionContext context) =>
                new GroupModel
                {
                    Id = source.Id,
                    Name = source.Name,
                    GroupUser = source.Users
                        .Select(u => new GroupUserModel
                        {
                            GroupId = source.Id,
                            UserId = u.Id
                        }).ToList()
                };
    }

    public class GroupFromAddResolver : ITypeConverter<GroupAddModel, GroupModel>
    {
        public GroupModel Convert(
            GroupAddModel source,
            GroupModel destination,
            ResolutionContext context) =>
                new GroupModel
                {
                    Name = source.Name,
                    GroupUser = source.Users
                        .Select(u => new GroupUserModel
                        {
                            GroupId = 0,
                            UserId = u.Id
                        }).ToList()
                };
    }

    public class GroupToManagerResolver : ITypeConverter<GroupModel, GroupManageModel>
    {
        public GroupManageModel Convert(
            GroupModel source,
            GroupManageModel destination,
            ResolutionContext context) =>
                new GroupManageModel
                {
                    Id = source.Id,
                    Name = source.Name,
                    Users = source.GroupUser
                        .Select(gu => new UserModelBase { Id = gu.UserId }).ToList()
                };
    }

    public class MyExpensesProfile : Profile
    {
        public MyExpensesProfile()
        {
            CreateMap<UserModel, UserModel>().ReverseMap();
            CreateMap<UserModel, UserModelBase>().ReverseMap();

            CreateMap<GroupModel, GroupModel>().ReverseMap();
            CreateMap<GroupGetModel, GroupModel>().ReverseMap();
            CreateMap<GroupModel, GroupGetFullModel>().ConvertUsing(new GroupToFullResolver());
            CreateMap<GroupManageModel, GroupModel>().ConvertUsing(new GroupFromManagerResolver());
            CreateMap<GroupAddModel, GroupModel>().ConvertUsing(new GroupFromAddResolver());
            CreateMap<GroupModel, GroupManageModel>().ConvertUsing(new GroupToManagerResolver());

            CreateMap<LabelModel, LabelModel>().ForMember(dest => dest.Group, act => act.Ignore());
            CreateMap<LabelGetFullModel, LabelModel>().ReverseMap();
            CreateMap<LabelManageModel, LabelModel>().ReverseMap();
            CreateMap<LabelAddModel, LabelModel>().ReverseMap();

            CreateMap<ExpenseModel, ExpenseModel>()
                .ForMember(dest => dest.Group, act => act.Ignore())
                .ForMember(dest => dest.Label, act => act.Ignore());
            CreateMap<ExpenseFullModel, ExpenseModel>().ReverseMap();
            CreateMap<ExpenseManageModel, ExpenseModel>().ReverseMap();
            CreateMap<ExpenseAddModel, ExpenseModel>().ReverseMap();
        }
    }
}
