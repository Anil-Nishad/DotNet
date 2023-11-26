using AutoMapper;
using DotNet.API.Models.Domain;
using DotNet.API.Models.DTO;
using DotNet.API.Repositories;
using DotNet.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalksRepository walksRepository;

        public WalksController(IMapper mapper,
                                IWalksRepository walksRepository)
        {
            this.mapper = mapper;
            this.walksRepository = walksRepository;
        }

        [HttpPost("CreateWalks")]
        public async Task<IActionResult> CreateWalks([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalksRequestDto);

            await walksRepository.CreateAsync(walkDomainModel);

            // Map Domain Model to DTO
            return Ok(mapper.Map<WalksDto>(walkDomainModel));
        }

        [HttpGet("GetAllWalks")]
        public async Task<IActionResult> GetAllWalks()
        {
            var walksDomainModel = await walksRepository.GetAllAsync();

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<WalksDto>>(walksDomainModel));
        }

        // GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet("GetAllWalksUsingFSP")]
        public async Task<IActionResult> GetAllWalksUsingFSP([FromQuery] string? filterOn, 
                                                             [FromQuery] string? filterQuery,
                                                             [FromQuery] string? sortBy, 
                                                             [FromQuery] bool? isAscending,
                                                             [FromQuery] int pageNumber = 1,
                                                             [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await walksRepository.GetAllAsyncFSP(filterOn, 
                                                                        filterQuery, 
                                                                        sortBy, 
                                                                        isAscending ?? true, 
                                                                        pageNumber, 
                                                                        pageSize);

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<WalksDto>>(walksDomainModel));
        }

        [HttpGet("GetWalkByID/{id:Guid}")]
        public async Task<IActionResult> GetWalkByID([FromRoute] Guid id)
        {
            var walkDomainModel = await walksRepository.GetAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain Model to DTO
            return Ok(mapper.Map<WalksDto>(walkDomainModel));
        }

        [HttpPut("UpdateWalk/{id:Guid}")]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, UpdateWalksRequestDto updateWalksRequestDto)
        {
            // Map DTO  to Domain Model
            var walkDomainModel = mapper.Map<Walk>(updateWalksRequestDto);

            walkDomainModel = await walksRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain Model to DTO
            return Ok(mapper.Map<WalksDto>(walkDomainModel));
        }

        [HttpDelete("DeleteWalk/{id:Guid}")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {
            var deletedWAlkDomainModel = await walksRepository.DeleteAsync(id);

            if (deletedWAlkDomainModel == null)
            {
                return NotFound(id);
            }

            //Map Domain Model to DTO
            return Ok(mapper.Map<WalksDto>(deletedWAlkDomainModel));
        }
    }
}
