using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.CustomActionFilters;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domain;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository  regionRepository, IMapper mapper)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        // GET Data from all Region
        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAll()
        {
            // Get All the data of region
            var regions = await _regionRepository.GetAllAsync();

            // return data          
            return Ok(_mapper.Map<List<RegionDto>>(regions));
        }

        // GET data by Id
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // GET region Domain Model from Database
            var regionDomain = await _regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
                return NotFound();

            // Map regionDomain model to regionDto            
            return Ok(_mapper.Map<RegionDto>(regionDomain));
        }

        //Create Data in Region
        // Post data
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] RegionRequestDto addRegionRequestDto)
        {
            // Map region request dto to region Domain
            var regionDomain = _mapper.Map<Region>(addRegionRequestDto);

            // Use Domain Model to create Region
            regionDomain = await _regionRepository.CreateAsync(regionDomain);

            // Map Dto to Domain Model
            var regionDto = _mapper.Map<RegionDto>(regionDomain);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] RegionRequestDto updateregionRequestDto)
        {
            //Map dto to Domain 
            var regionDomain = _mapper.Map<Region>(updateregionRequestDto);

            // Check if region exists
            regionDomain = await _regionRepository.UpdateAsync(id, regionDomain);

            if (regionDomain == null)
                return NotFound();

            // Map Domain Model to Dto          
            return Ok(_mapper.Map<RegionDto>(regionDomain));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomain = await _regionRepository.DeleteAsync(id);
            if (regionDomain == null)
                return NotFound();
            
            //return Deleted region 
            // Map Domain to dto
            return Ok(_mapper.Map<RegionDto>(regionDomain));
        }

    }
}
