using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GameManager.DAL;
using GameManager.Models;
using System.Collections.ObjectModel;

namespace GameManager.Controllers
{
    public class GamesController : ApiController
    {
        private GameManagerContext db = new GameManagerContext();

        // GET: api/Games
        //[Authorize(Roles = "Employee")]
        public async Task<IHttpActionResult> Get()
        {
            List<GameDetailsDTO> games = new List<GameDetailsDTO>();
                games = await db.Games.Select(g =>
                    new GameDetailsDTO
                    {
                        Id = g.Id,
                        Name = g.Name,
                        ReleaseDate = g.ReleaseDate,
                        Price = g.Price,
                        InventoryCount = g.InventoryCount
                    }).ToListAsync();  
          
            if(games.Count > 0){
                foreach(GameDetailsDTO eachgame in games){
                    eachgame.Genres = GenreList(eachgame.Id);
                    eachgame.Tags = TagList(eachgame.Id);
                }
            }
            return Ok(games);
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            List<GameDetailsDTO> games = new List<GameDetailsDTO>();
            games = await db.Games.Where(g => g.Id == id).Select(g =>
                new GameDetailsDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    ReleaseDate = g.ReleaseDate,
                    Price = g.Price,
                    InventoryCount = g.InventoryCount
                }).ToListAsync();

            if (games.Count > 0)
            {
                foreach (GameDetailsDTO eachgame in games)
                {
                    eachgame.Genres = GenreList(eachgame.Id);
                    eachgame.Tags = TagList(eachgame.Id);
                }
            }
            return Ok(games);
        }

        // GET: api/Games/5
        //[Authorize(Roles = "StoreAdmin")]
        [ResponseType(typeof(Game))]
        public async Task<IHttpActionResult> GetGamebyId(int game)
        {
            Game games = await db.Games.FindAsync(game);
            if (games == null)
            {
                return NotFound();
            }

            return Ok(games);
        }

        public async Task<IHttpActionResult> GetGamebyGenre(int genre)
        {
            var gamegenre = await db.Genres.Where(u => u.Id == genre)
            .SelectMany(
                g => g.Games,
                (g, game) => new GameDetailsDTO
                {
                    Id = game.Id,
                    Name = game.Name,
                    ReleaseDate = game.ReleaseDate,
                    Price = game.Price,
                    InventoryCount = game.InventoryCount,
                    }).ToListAsync();

            if(gamegenre.Count > 0){
                foreach (GameDetailsDTO eachgame in gamegenre)
                {
                    eachgame.Genres = GenreList(eachgame.Id);
                    eachgame.Tags = TagList(eachgame.Id);
                }
                return Ok(gamegenre);
            }
            else
            {
                return NotFound();
            }
        }

        public string GenreList(int gameId){
            string genrelist = "";
            
            using (var db = new GameManagerContext()){
                var list = db.Games.Where(u => u.Id == gameId).
                           SelectMany(
                           g => g.Genre, (genre, game) =>
                                new GameGenreDTO
                                {
                                    Genres = game.Name
                                }).ToList();

                foreach (GameGenreDTO game in list)
                {
                    genrelist += game.Genres + ", ";
                }
                if(genrelist.Length > 0){
                    genrelist = genrelist.Substring(0, genrelist.Length - 2);

                }
            }
            return genrelist;
        }

        public string TagList(int gameId)
        {
            string taglist = "";

            using (var db = new GameManagerContext())
            {
                var list = db.Games.Where(u => u.Id == gameId).
                           SelectMany(
                           g => g.Tags, (tag, game) =>
                                new GameTagDTO
                                {
                                    Tags = game.Name
                                }).ToList(); 

                foreach (GameTagDTO game in list)
                {
                    taglist += game.Tags + ", ";
                }
                if(taglist.Length > 0){
                    taglist = taglist.Substring(0, taglist.Length - 2);
                }
            }
            return taglist;
        }

        // PUT: api/Games/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGame(int id, Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != game.Id)
            {
                return BadRequest();
            }

            db.Entry(game).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult PostGame(NewGameDTO game)
        {
            if(ModelState.IsValid){
                ICollection<string> genrelist = game.Genres.Split(new[]{',', ' '}, StringSplitOptions.RemoveEmptyEntries).ToList();
                ICollection<string> taglist = game.Tags.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                ICollection<Genre> finalgenre = new Collection<Genre>();
                ICollection<Tag> finaltag = new Collection<Tag>();

                Game newgame = new Game();
                newgame.Name = game.Name;              
                newgame.Price = Convert.ToDecimal(game.Price);
                newgame.ReleaseDate = game.ReleaseDate;
                newgame.InventoryCount = game.InventoryCount;

                foreach(string genrecheck in genrelist){
                    if (duplicate_genre_check(genrecheck) != 0){
                        finalgenre.Add(matching_genre(genrecheck));
                    }
                    else
                    {
                        finalgenre.Add(new Genre
                            {
                                Name = genrecheck
                             }
                         );
                    }
                }

                foreach (string tagcheck in taglist)
                {
                    if (duplicate_tag_check(tagcheck) != 0)
                    {
                        finaltag.Add(matching_tag(tagcheck));
                    }
                    else
                    {
                        finaltag.Add(new Tag
                            {
                                Name = tagcheck
                            }
                        );
                    }
                }
                newgame.Genre = finalgenre;
                newgame.Tags = finaltag;
                db.Games.Add(newgame);
                db.SaveChanges();

                //LOCATION
                IHttpActionResult response;
                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
                responseMsg.Content = new StringContent("Game added successfully. ID : " + newgame.Id);
                //string uri = Url.Link("GetGamesById", new { id = newgame.Id });
                //responseMsg.Headers.Location = new Uri(uri);
                response = ResponseMessage(responseMsg);
                return response;
            }

            return BadRequest();
        }


        public int duplicate_genre_check(string genre)
        {
            var temp_genre = db.Genres.FirstOrDefault(u => u.Name == genre);
            
            if(temp_genre != null){
                return temp_genre.Id;
            }
            return 0;
        }

        public Genre matching_genre(string genre)
        {
            var temp_genre = db.Genres.FirstOrDefault(u => u.Name == genre);
            if(temp_genre != null){
                return temp_genre;
            }
            return null;
        }

        public int duplicate_tag_check(string tag)
        {
            var temp_tag = db.Tags.FirstOrDefault(u => u.Name == tag);

            if (temp_tag != null)
            {
                return temp_tag.Id;
            }
            return 0;
        }

        public Tag matching_tag(string tag)
        {
            var temp_tag = db.Tags.FirstOrDefault(u => u.Name == tag);
            if (temp_tag != null)
            {
                return temp_tag;
            }
            return null;
        }

        // DELETE: api/Games/5
        [ResponseType(typeof(Game))]
        public async Task<IHttpActionResult> DeleteGame(int id)
        {
            Game game = await db.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            db.Games.Remove(game);
            await db.SaveChangesAsync();

            return Ok(game);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GameExists(int id)
        {
            return db.Games.Count(e => e.Id == id) > 0;
        }
    }
}