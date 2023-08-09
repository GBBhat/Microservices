using AutoMapper;
using CommandsService.CommandData;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly IcommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(IcommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Getting all the commands by platform ID: {platformId} ");
            if(!_repository.PlatformExists(platformId)){
                return NotFound();
            }
            var commandItems = _repository.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        [HttpGet("{commandId}", Name="GetCommandForPlatform")]    
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"-->Getting Command for a platform Id & Command Id: {platformId/ commandId}");
            if(!_repository.PlatformExists(platformId)){
                return NotFound();
            }
            var command = _repository.GetCommand(platformId, commandId);
            if(command == null){
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForplatform(int platformId, CommandCreateDto cmd)
        {
            Console.WriteLine("--> Creating the command");

            if(!_repository.PlatformExists(platformId))
            {
               return NotFound();
            }

            var command = _mapper.Map<Command>(cmd);
            _repository.CreateCommand(platformId,command);
            _repository.SaveChanges();

             var commandReadDto = _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform),
                new{platformId = platformId, commandId = commandReadDto.Id}, command);

        }

    }
}