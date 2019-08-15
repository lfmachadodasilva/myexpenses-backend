using System;
using System.Linq;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Properties;
using lfmachadodasilva.MyExpenses.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IWebSettings _webSettings;

        public GroupsController(
            IGroupService groupService,
            IWebSettings webSettings)
        {
            _groupService = groupService;
            _webSettings = webSettings;
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(GroupDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _groupService.GetAllAsync(_webSettings.DefaultUserId));
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //[ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var task = Task.Run(() =>
        //    {
        //        return _fakeDatabase.Groups.FirstOrDefault(x => x.Id.Equals(id));

        //    });
        //    return Ok(await task);
        //}

        //// POST api/values
        //[HttpPost]
        //[ProducesResponseType(typeof(GroupDto), StatusCodes.Status201Created)]
        //public async Task<IActionResult> Add([FromBody] GroupDto value)
        //{
        //    if (value.Name == "duplicate")
        //    {
        //        return Conflict(Resource.ErrorDuplicate);
        //    }
        //    var task = Task.Run(() =>
        //    {
        //        value.Id = _fakeDatabase.Groups.Count;

        //        var withValues = new GroupDto
        //        {
        //            Id = _fakeDatabase.Groups.Count,
        //            Name = value.Name,
        //            Users = value.Users
        //        };

        //        _fakeDatabase.Groups.Add(withValues);

        //        return withValues;
        //    });

        //    return Ok(await task);
        //}

        //// PUT api/values/5
        //[HttpPut]
        //[ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        //public async Task<IActionResult> Edit([FromBody] GroupDto value)
        //{
        //    if (value.Name == "duplicate")
        //    {
        //        return Conflict(Resource.ErrorDuplicate);
        //    }

        //    var task = Task.Run(() =>
        //    {
        //        var obj = _fakeDatabase.Groups.FirstOrDefault(x => x.Id.Equals(value.Id));
        //        obj.Name = value.Name;
        //        obj.Users = value.Users;

        //        return obj;
        //    });

        //    return Ok(await task);
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        //public async Task Delete(int id)
        //{
        //    var task = Task.Run(() =>
        //    {
        //        var obj = _fakeDatabase.Groups.FirstOrDefault(x => x.Id.Equals(id));
        //        _fakeDatabase.Groups.Remove(obj);
        //    });
        //    await task;
        //}
    }
}
