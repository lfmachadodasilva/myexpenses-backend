using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Models.Domains;
using MyExpenses.Models.Dtos;
using MyExpenses.Services;

namespace MyExpenses.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var domain = await _userService.GetAllAsync();
            var dto = _mapper.Map<ICollection<UserDto>>(domain);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string id)
        {
            var domain = await _userService.GetByIdAsync(id);
            var dto = _mapper.Map<UserDto>(domain);
            return Ok(dto);
        }

        // POST api/label
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] UserDto dto)
        {
            var domain = _mapper.Map<UserDomain>(dto);
            var dtoAdded = await _userService.AddAsync(domain);
            return Ok(_mapper.Map<UserDto>(dtoAdded));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] UserDto dto)
        {
            var domain = _mapper.Map<UserDomain>(dto);
            var dtoUpdated = await _userService.UpdateAsync(domain);
            return Ok(_mapper.Map<UserDto>(dtoUpdated));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _userService.DeleteAsync(id));
        }
    }
}
