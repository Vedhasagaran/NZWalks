using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.CustomActionFilters;
using NZWalks.Api.Models.Domain;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }

        // Create Walk data
        //POST : /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map walkdto to walk domain model
            var walkDomain = _mapper.Map<Walk>(addWalkRequestDto);
            await _walkRepository.CreateAsync(walkDomain);

            return Ok(_mapper.Map<WalkDto>(walkDomain));
        }

        //GET all Walk
        //GET : /api/walks/filterOn=Name&filterQuery=park&sortBy=Name&isAscending=true&pageNumber=2&pageSize=5
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending , [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000)
        {
            var walks = await _walkRepository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending ?? true,
                              pageNumber,pageSize);

            return Ok(_mapper.Map<List<WalkDto>>(walks));
        }

        //GET Walk by ID
        //GET : /api/walks/{id:Guid}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomain = await _walkRepository.GetByIdAsync(id);
            if (walkDomain == null)
                return NotFound();

            return Ok(_mapper.Map<WalkDto>(walkDomain));
        }

        //Update Walk by Id
        //PUT : /api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkDomain = _mapper.Map<Walk>(updateWalkRequestDto);

            walkDomain = await _walkRepository.UpdateAsync(id, walkDomain);

            if (walkDomain == null)
                return NotFound();

            return Ok(_mapper.Map<WalkDto>(walkDomain));
        }

        //Delete walk by Id
        //DELETE : /api/walks/{id}        
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomain = await _walkRepository.DeleteAsync(id);
            if(walkDomain == null)
                return NotFound();

            return Ok(_mapper.Map<WalkDto>(walkDomain));
        }
    }
}
