using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GameManager.Models;
using GameManager.DAL;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GameManager.Controllers
{
    public class CategoriesController : ApiController
    {
        private GameManagerContext db = new GameManagerContext();

        public async Task<IHttpActionResult> GetCategoriesByGame(int? game)
        {
            if(game != null){
                var categoriesbygame = await db.Games.Where(u => u.Id == game)
                     .Select(g => new GameCategoriesDTO
                     {
                         Id = g.Id,
                         Name = g.Name
                     }).ToListAsync();

                if (categoriesbygame.Count > 0)
                {
                    GamesController gamesctrl = new GamesController();

                    foreach (GameCategoriesDTO eachgame in categoriesbygame)
                    {
                        eachgame.Genres = gamesctrl.GenreList(eachgame.Id);
                        eachgame.Tags = gamesctrl.TagList(eachgame.Id);
                    }
                    return Ok(categoriesbygame);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}