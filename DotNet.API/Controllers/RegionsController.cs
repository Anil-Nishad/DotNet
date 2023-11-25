using DotNet.API.Data;
using DotNet.API.Models.Domain;
using DotNet.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet.API.Controllers
{
    //https://localhost:portnumber/api/regions
    //Attribute
    [Route("api/[controller]")]
    //Api controller attribute will tell this application that this controller is for API use.
    //So it will automatically validates the modal state and gives a 400 response back
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Get All Hard Coded Regions
        // Get: https://localhost:portnumber/api/regions
        [HttpGet("GetAllHardCodedRegions")]
        public IActionResult GetAllHardCodedRegions()
        {
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.NewGuid(),
                    Name = "Delhi",
                    Code = "DEL",
                    RegionImageUrl = "https://images.pexels.com/photos/9493792/pexels-photo-9493792.jpeg?auto=compress&cs=tinysrgb&w=600&lazy=load"
                },
                new Region
                {
                    Id = Guid.NewGuid(),
                    Name = "Chandigarh",
                    Code = "CHD",
                    RegionImageUrl = "https://images.pexels.com/photos/18947084/pexels-photo-18947084/free-photo-of-town-hall-in-quedlinburg.jpeg?auto=compress&cs=tinysrgb&w=600&lazy=load"
                }
            };
            return Ok(regions);
        }

        [HttpGet("GetRegions")]
        public IActionResult GetRegions()
        {
            var regions = dbContext.Regions.ToList();
            return Ok(regions);
        }

        [HttpGet]
        //[HttpGet("GetRegionById")]
        //[Route("{id}")]
        [Route("{id:Guid}")]
        //public IActionResult GetRegionById(Guid id)
        public IActionResult GetRegionById([FromRoute] Guid id)
        {
            // Find method only take ID property.
            var region = dbContext.Regions.Find(id);
            // or
            // var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(region);
        }

        [HttpGet("GetRegionsUsingDto")]
        public IActionResult GetRegionsUsingDto()
        {
            // Get Data From Database - Domain Models
            var regionsDomain = dbContext.Regions.ToList();

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

        [HttpGet("GetRegionByIdUsingDto/{id:Guid}")]
        //[Route("{id:Guid}")]
        //public IActionResult GetRegionByIdUsingDto(Guid id)
        public IActionResult GetRegionByIdUsingDto(Guid id)
        {
            // Get Region Domain Model From Database
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

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

        [HttpPost("CreateRegion")]
        public IActionResult CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or Convert DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // Use Domain Model to create Region
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            // Map Domain Model back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetRegionByIdUsingDto), new {id = regionDto.Id}, regionDto);

        }

        [HttpPut("UpdateRegion/{id:Guid}")]
        public IActionResult UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Check if Region exists
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Map DTO to Domain Model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            dbContext.SaveChanges();

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

        [HttpDelete("DeleteRegion/{id:Guid}")]
        public IActionResult DeleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id ==id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Delete Region
            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();

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
