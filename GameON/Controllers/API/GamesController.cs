using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GameON.Models;
using System.Web.Mvc;
using GameON.Dtos;
using AutoMapper;
using System.Data.Entity;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace GameON.Controllers.API
{
    public class GamesController : ApiController
    {
        private ApplicationDbContext _context;

        public GamesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/games
        public IEnumerable<GameDto> GetGames()
        {
            return _context.Games.Include(c => c.GameType).ToList().Select(Mapper.Map<Game,GameDto>);
        }

        // GET /api/games/1
        public IHttpActionResult GetGame(int id)
        {
            var game = _context.Games.SingleOrDefault(x => x.Id == id);

            if (game == null)
                return NotFound();

            return Ok(Mapper.Map<Game,GameDto>(game));
            
        }

        // POST /api/games
        [System.Web.Http.Authorize(Roles = "Admin")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult CreateGame(GameDto gameDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var game = Mapper.Map<GameDto, Game>(gameDto);

            _context.Games.Add(game);
            _context.SaveChanges();

            gameDto.Id = game.Id;

            return Created(new Uri(Request.RequestUri + "/" + game.Id),game);
        }

        // PUT /api/games/1
        [System.Web.Http.Authorize(Roles = "Admin")]
        [System.Web.Http.HttpPut]
        public void UpdateGame(int id, GameDto gameDto)
        {
            if (!ModelState.IsValid)
                throw new HttpRequestException(HttpStatusCode.BadRequest.ToString());

            var gameInDb = _context.Games.SingleOrDefault(x => x.Id == id);

            if (gameInDb == null)
                throw new HttpRequestException(HttpStatusCode.NotFound.ToString());

            Mapper.Map(gameDto, gameInDb);

            _context.SaveChanges();
        }

        //DELETE /api/games/1
        [System.Web.Http.Authorize(Roles = "Admin")]
        [System.Web.Http.HttpDelete]
        public void DeleteGame(int id)
        {
            var gameInDb = _context.Games.SingleOrDefault(x => x.Id == id);

            if (gameInDb == null)
                throw new HttpRequestException(HttpStatusCode.NotFound.ToString());

            _context.Games.Remove(gameInDb);
            _context.SaveChanges();
        }

        
    }
}
