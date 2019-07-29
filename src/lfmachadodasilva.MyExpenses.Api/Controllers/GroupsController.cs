using System;
using System.Linq;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        public GroupsController(FakeDatabase db)
        {
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(GroupDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            const long userId = 0;
            var task = Task.Run(() =>
            {
                return FakeDatabase.Groups.Where(x => x.Users.Any(y => y.Id.Equals(userId)));
            });
            return Ok(await task);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var task = Task.Run(() =>
            {
                return FakeDatabase.Groups.FirstOrDefault(x => x.Id.Equals(id));

            });
            return Ok(await task);
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] GroupDto value)
        {
            if (value.Name == "duplicate")
            {
                return Conflict(Resource.ErrorDuplicate);
            }
            var task = Task.Run(() =>
            {
                value.Id = FakeDatabase.Groups.Count;

                var withValues = new GroupDto
                {
                    Id = FakeDatabase.Groups.Count,
                    Name = value.Name,
                    Users = value.Users
                };

                FakeDatabase.Groups.Add(withValues);

                return withValues;
            });

            return Ok(await task);
        }

        // PUT api/values/5
        [HttpPut]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromBody] GroupDto value)
        {
            if (value.Name == "duplicate")
            {
                return Conflict(Resource.ErrorDuplicate);
            }

            var task = Task.Run(() =>
            {
                var obj = FakeDatabase.Groups.FirstOrDefault(x => x.Id.Equals(value.Id));
                obj.Name = value.Name;
                obj.Users = value.Users;

                return obj;
            });

            return Ok(await task);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task Delete(int id)
        {
            var task = Task.Run(() =>
            {
                var obj = FakeDatabase.Groups.FirstOrDefault(x => x.Id.Equals(id));
                FakeDatabase.Groups.Remove(obj);
            });
            await task;
        }
    }
}
