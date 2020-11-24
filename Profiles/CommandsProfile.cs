using AutoMapper;
using WebApiTest.DTOs;
using WebApiTest.Models;

namespace WebApiTest.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            //Source to -> Destination (Target)
            CreateMap<Command, CommandReadDto>();

            CreateMap<CommandCreateDto, Command>();

            CreateMap<CommandUpdateDto, Command>();

            CreateMap<Command, CommandUpdateDto>();

        }
    }
}