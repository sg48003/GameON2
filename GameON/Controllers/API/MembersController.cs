using AutoMapper;
using GameON.Dtos;
using GameON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GameON.Controllers.API
{
    public class MembersController : ApiController
    {
        private ApplicationDbContext _context;

        public MembersController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/games
        public IEnumerable<UserDto> GetMembers()
        {
            return _context.Users.ToList().Select(Mapper.Map<ApplicationUser, UserDto>);
        }

    }
}
