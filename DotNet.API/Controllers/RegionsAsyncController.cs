using DotNet.API.Data;
using DotNet.API.Models.Domain;
using DotNet.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsAsyncController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsAsyncController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("GetRegionsAsync")]
        public async Task<IActionResult> GetRegionsAsync()
        {
            var regions = await dbContext.Regions.ToListAsync();
            return Ok(regions);
        }

        [HttpGet]
        //[HttpGet("GetRegionById")]
        //[Route("{id}")]
        [Route("{id:Guid}")]
        //public IActionResult GetRegionById(Guid id)
        public async Task<IActionResult> GetRegionByIdAsync([FromRoute] Guid id)
        {
            // Find method only take ID property.
            var region = await dbContext.Regions.FindAsync(id);
            // or
            // var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(region);
        }

        [HttpGet("GetRegionsUsingDtoAsync")]
        public async Task<IActionResult> GetRegionsUsingDtoAsync()
        {
            // Get Data From Database - Domain Models
            var regionsDomain = await dbContext.Regions.ToListAsync();

            // Map Domain Models To DTOs
            var regionsDto = new List<RegionDto>();
            foreach (var region in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }
            // Return DTOs
            return Ok(regionsDto);
        }

        [HttpGet("GetRegionByIdUsingDtoAsync/{id:Guid}")]
        //[Route("{id:Guid}")]
        //public IActionResult GetRegionByIdUsingDto(Guid id)
        public async Task<IActionResult> GetRegionByIdUsingDtoAsync(Guid id)
        {
            // Get Region Domain Model From Database
            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map/Convert Region Domain Domain Model To Region DTO
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            // Return DTO back to  client
            return Ok(regionDto);
        }

        [HttpPost("CreateRegionAsync")]
        public async Task<IActionResult> CreateRegionAsync([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or Convert DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // Use Domain Model to create Region
            await dbContext.Regions.AddAsync(regionDomainModel);
            await dbContext.SaveChangesAsync();

            // Map Domain Model back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetRegionByIdUsingDtoAsync), new { id = regionDto.Id }, regionDto);

        }

        [HttpPut("UpdateRegionAsync/{id:Guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Check if Region exists
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Map DTO to Domain Model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            // Convert Domain Model
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        [HttpDelete("DeleteRegionAsync/{id:Guid}")]
        public async Task<IActionResult> DeleteRegionAsync([FromRoute] Guid id)
        {
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Delete Region
            dbContext.Regions.Remove(regionDomainModel);
            await dbContext.SaveChangesAsync();

            // return deleted Region back
            // map Domain Model to DTO

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
