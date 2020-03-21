using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Models.Domains;
using MyExpenses.Models.Dtos;
using MyExpenses.Services;

namespace MyExpenses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var domain = await _groupService.GetAllAsync();
            var dto = _mapper.Map<ICollection<GroupDto>>(domain);
            return Ok(dto);
        }

        [HttpGet("detailed")]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWithDetails()
        {
            var domain = await _groupService.GetAllWithDetailsAsync();
            var dto = _mapper.Map<ICollection<GroupDetailsDto>>(domain);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string id)
        {
            var domain = await _groupService.GetByIdAsync(id);
            var dto = _mapper.Map<GroupDetailsDto>(domain);
            return Ok(dto);
        }

        // POST api/label
        [HttpPost]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] GroupDto dto)
        {
            var domain = _mapper.Map<GroupDomain>(dto);
            var dtoAdded = await _groupService.AddAsync(domain);
            return Ok(_mapper.Map<GroupDto>(dtoAdded));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] GroupDto dto)
        {
            var domain = _mapper.Map<GroupDomain>(dto);
            var dtoUpdated = await _groupService.UpdateAsync(domain);
            return Ok(_mapper.Map<GroupDto>(dtoUpdated));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _groupService.DeleteAsync(id));
        }
    }
}
