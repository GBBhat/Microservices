using System;
using AutoMapper;
using CommandsService.CommandData;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controller
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IcommandRepo _repository;
        private readonly IMapper _mapper;

        public PlatformsController(IcommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatform()
        {
            Console.WriteLine("--> Getting platforms from Commands Service");

            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("Inbound Post # Command Service");

            return Ok("Inbound test of form Platforms Controller");
        }

    }
}