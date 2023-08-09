using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformServcie.AsyncDataServices;
using PlatformServcie.Dtos;
using PlatformServcie.Models;
using PlatformServcie.PlatformData;
using PlatformServcie.SyncDataClient.Http;

namespace PlatformServcie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _reposiroty;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepo repository, 
        IMapper mapper,
        ICommandDataClient commandDataClient,
        IMessageBusClient messageBusClient)
        {
            _reposiroty = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            Console.WriteLine("--Getting Platforms--");

            var platformItems = _reposiroty.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name="GetPlatformById")]    
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _reposiroty.GetPlatformById(id);
            if(platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _reposiroty.CreatePlatform(platformModel);
            _reposiroty.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            //send message synchronously
            try{
                await _commandDataClient.SendPlatformtoCommand(platformReadDto);
            }
            catch(Exception ex){
                Console.WriteLine($"Could not send synchronously: {ex.Message}");
            }

            //send message asynchronously
            try{
                var platformPublishDto = _mapper.Map<PlatformPublishDto>(platformReadDto);
                platformPublishDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById),new{Id = platformModel.Id}, platformReadDto);
        }
    }
}