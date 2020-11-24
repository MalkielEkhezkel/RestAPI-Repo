using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApiTest.Data;
using WebApiTest.DTOs;
using WebApiTest.Models;
using WebApiTest.QueryParametersPaging;


namespace WebApiTest.Controllers
{
    //?api-version=1.0
    [ApiVersion("1.0")] // old version
    [ApiVersion("1.1")]

    //api/commands
    //[Route("api/[controller]")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CommandsController : ControllerBase
    {
        private readonly IWebApiTestRepository _repository;
        private readonly IMapper _mapper;

        //the standart way of capturing a category
        //private readonly ILogger<CommandsController> _logger;
        
        // A new way to capture for making my own name at the capture 
        private readonly ILogger _logger;

        //the standart way of capturing a category
        // public CommandsController(IWebApiTestRepository repository, IMapper mapper,
        //                             ILogger<CommandsController> logger)

        // A new way to capture for making my own name at the capture 
        public CommandsController(IWebApiTestRepository repository, IMapper mapper,
                                    ILoggerFactory factory)
        {
            _repository = repository; 
            _mapper = mapper; 
            _logger = factory.CreateLogger("ControllerCategory");
            _logger.LogInformation("Controller started");

            _logger.LogTrace("====>>>>>Trace Log");// very detailed Log
            _logger.LogDebug("====>>>>>Debug Log");// about data and value
            _logger.LogInformation("====>>>>>Information Log");// for flow // used log
            _logger.LogWarning("====>>>>>Warning Log");//for cached exseption that cached  // used log
            _logger.LogError("====>>>>>Error Log");//unexpected and unhandled data
            _logger.LogCritical("====>>>>>Critical Log");//application is crashing or losing data or something critical


            _logger.LogWarning("====>>>>>Warning Log at {Time}", DateTime.UtcNow);

            try
            {
                throw new Exception("You forgot to catch me");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Critical exception at time {Time}", DateTime.UtcNow);
            }


        }
        
        //private readonly MockWebApiTestRepository _repository = new MockWebApiTestRepository();

        //GET api/commands
        [HttpGet]
        //[ProducesResponseType(200)]
        //public ActionResult <IEnumerable<Command>> GetAllCommands()
        public ActionResult <IEnumerable<CommandReadDto>> GetAllCommands([FromQuery] CommandQueryParameters commandQueryParameters)
        {
            _logger.LogInformation("=====>>> GetAllCommands");

            var commandItems = _repository.GetAllCommands(commandQueryParameters);

            Response.Headers.Add("x-Pagination", 
                    JsonConvert.SerializeObject(new { totalCount = _repository.Count() }));
            //return Ok(commandItems);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        //GET api/commands
        [HttpGet]
        //[ProducesResponseType(200)]
        //public ActionResult <IEnumerable<Command>> GetAllCommands()
        [MapToApiVersion("1.1")] // v1.1 specific action for GET api/values endpoint
        public ActionResult <IEnumerable<CommandReadDto>> GetAllCommandsV1_1([FromQuery] CommandQueryParameters commandQueryParameters)
        {
            var commandItems = _repository.GetAllCommands(commandQueryParameters);

            Response.Headers.Add("x-Pagination", 
                    JsonConvert.SerializeObject(new { totalCount = _repository.Count() }));
            //return Ok(commandItems);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        //GET api/commands/{id}
        [HttpGet("{id}", Name="GetCommandById")]
        //public ActionResult <Command> GetCommandById(int id)
        public ActionResult <CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if(commandItem!=null)
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
               // return Ok(commandItem);
            return NotFound();
        }

        //POST api/commands
        [HttpPost]
        public ActionResult <CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            _logger.LogInformation("=====>>> CreateCommand");

            var commandModel = _mapper.Map<Command>(commandCreateDto);

            _logger.LogInformation("=====>>> CreateCommand Map");
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            _logger.LogInformation("=====>>> CreateCommand SaveChanges");
            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            _logger.LogInformation("=====>>> CreateCommand SaveChanges");

            //return Ok(commandReadDto);
            return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDto.Id}, commandReadDto);
        }

        //PUT api/command/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepository = _repository.GetCommandById(id);
            if(commandModelFromRepository == null)
                return NotFound();
            
            _mapper.Map(commandUpdateDto, commandModelFromRepository);

            _repository.UpdateCommand(commandModelFromRepository);
            _repository.SaveChanges();

            return NoContent();
        }

        //PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepository = _repository.GetCommandById(id);
            if(commandModelFromRepository == null)
                return NotFound();
            
            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepository);

            patchDoc.ApplyTo(commandToPatch, ModelState);
            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFromRepository);

            _repository.UpdateCommand(commandModelFromRepository);
            _repository.SaveChanges();

            return NoContent();
        }

        //DELETE api/command/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
             var commandModelFromRepository = _repository.GetCommandById(id);
            if(commandModelFromRepository == null)
                return NotFound();
            
            _repository.DeleteCommand(commandModelFromRepository);
            _repository.SaveChanges();
            
            return NoContent();
        }


        [HttpGet("About")]
        public ContentResult About()
        {
            return Content("An API test");
        }

        [HttpGet("version")]
        public string Version()
        {
            return "Version 1.0";
        }
    }
}