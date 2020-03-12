using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Services;

namespace MyExpenses.Controllers
{
    public abstract class MyControllerBase : ControllerBase
    {
        private readonly IGroupService _groupService;

        protected MyControllerBase(IGroupService groupService)
        {
            _groupService = groupService;
        }

        protected Task<bool> Validate(long groupId)
        {
            var userId = User.FindFirst("user_id")?.Value;
            return _groupService.Validate(groupId, userId);
        }
    }
}
