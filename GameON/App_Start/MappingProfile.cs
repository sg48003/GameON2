using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using GameON.Dtos;
using GameON.Models;

namespace GameON.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Domain to Dto
            Mapper.CreateMap<Game, GameDto>();
            Mapper.CreateMap<GameType, GameTypeDto>();
            Mapper.CreateMap<ApplicationUser, UserDto>();

            //Dto to Domain
            Mapper.CreateMap<GameDto, Game>().ForMember(c => c.Id, opt => opt.Ignore());
            Mapper.CreateMap<GameTypeDto, GameType>().ForMember(c => c.Id, opt => opt.Ignore());
            Mapper.CreateMap<UserDto, ApplicationUser>().ForMember(c => c.Id, opt => opt.Ignore());
        }
    }
}