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
    }
}
