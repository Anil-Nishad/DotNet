using AutoMapper;
using DotNet.API.CustomActionFilters;
using DotNet.API.Models.Domain;
using DotNet.API.Models.DTO;
using DotNet.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace DotNet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionAutomapperController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionAutomapperController> logger;

        public RegionAutomapperController(IRegionRepository regionRepository, 
                                            IMapper mapper,
                                            ILogger<RegionAutomapperController> logger)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet("GetAllRegions")]
        //[Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> GetAllRegions()
        {
            try
            {
                logger.LogInformation("GetAllRegions Action Method was invoked");
                logger.LogWarning("This is a warning log");
                logger.LogError("This ia a error log");

                var regionsDomain = await regionRepository.GetAllAsync();

                // Map Domain Models To DTOs
                var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

                logger.LogInformation($"Finished GetAllRegions request with Data: {JsonSerializer.Serialize(regionsDomain)}");

                // Return DTOs
                return Ok(regionsDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return Problem("Something Went Wrong", null, (int)HttpStatusCode.InternalServerError);
                //return Problem(ex.Message, null, (int)HttpStatusCode.InternalServerError);
                //return BadRequest(ex.Message);
                //throw;
            }
            
        }

        [HttpGet("GetRegionById/{id:Guid}")]
        [Authorize(Roles = "Reader")]
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
        [Authorize(Roles = "Writer")]
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
        [Authorize(Roles = "Writer")]
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
        [Authorize(Roles = "Writer")]
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
