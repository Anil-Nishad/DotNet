using AutoMapper;
using DotNet.API.CustomActionFilters;
using DotNet.API.Models.Domain;
using DotNet.API.Models.DTO;
using DotNet.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionAutomapperController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionAutomapperController(IRegionRepository regionRepository, 
                                            IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet("GetAllRegions")]
        public async Task<IActionResult> GetAllRegions()
        {
            var regionsDomain = await regionRepository.GetAllAsync();

            // Map Domain Models To DTOs
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            // Return DTOs
            return Ok(regionsDto);
        }

        [HttpGet("GetRegionById/{id:Guid}")]
        public async Task<IActionResult> GetRegionById(Guid id)
        {
            // Get Region Domain Model From Database
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map/Convert Region Domain Domain Model To Region DTO
            var regionDto = mapper.Map<RegionDto>(regionDomain);

            // Return DTO back to  client
            return Ok(regionDto);
        }

        [HttpPost("CreateRegion")]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            if (ModelState.IsValid)
            {
                // Map or Convert DTO to Domain Model
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                // Use Domain Model to create Region
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                // Map Domain Model back to DTO
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("UpdateRegion/{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert Domain Model To DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

        [HttpDelete("DeleteRegion/{id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // return deleted Region back
            // map Domain Model to DTO

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

    }
}
