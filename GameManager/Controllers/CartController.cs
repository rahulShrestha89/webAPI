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
using System.Web;
using System.Collections.ObjectModel;

namespace GameManager.Controllers
{
    public class CartController : ApiController
    {
        private GameManagerContext db = new GameManagerContext();

        // GET: api/Cart
        [Authorize]
        [HttpGet]
        [Route("api/cart")]
        public async Task<IHttpActionResult> GetCarts()
        {
            string userEmail = HttpContext.Current.User.Identity.Name;
            int customerid = IdFromEmail(userEmail);

            var activeCart = db.Carts.Where(c => c.CustomerId == customerid && c.isProcessed == false).Select(p =>
                    new ViewCart
                    {
                        CustomerId = userEmail,
                        GamesInCart = p.Games
                    }
                ).ToList();

            if(activeCart != null){
                foreach(ViewCart cart in activeCart){
                    cart.TotalPrice = TotalPrice((GamesInCart(customerid)));
                }
            }
            return Ok(activeCart);
        }

        public ICollection<Game> GamesInCart(int userid)
        {
            var cart = db.Carts.FirstOrDefault(u => u.CustomerId == userid);
            if(cart != null){
                return cart.Games;
            }
            return null;
        }


        public static decimal TotalPrice(ICollection<Game> Games)
        {
            decimal total = 0;
            foreach(Game game in Games){
                total += game.Price;
            }
            return total;
        }

        [Authorize]
        [HttpPost]
        [Route("api/cart")]
        // POST api/cart
        public async Task<IHttpActionResult> POST(AddGamesToCartDTO games)
        {
            if (ModelState.IsValid)
            {
                ICollection<string> tempgames = games.Games.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                ICollection<Game> dbgames = new Collection<Game>(); ;

                
                string userEmail = HttpContext.Current.User.Identity.Name;
                int customerid = IdFromEmail(userEmail);

                //User has unprocessed cart
                if (has_oldcart(customerid))
                {
                    //New Cart
                    Cart newcart = new Cart();
                    newcart.isProcessed = false;
                    newcart.CustomerId = customerid;

                    //Add Game
                    foreach (string tempgame in tempgames)
                    {
                        if (GameById(Convert.ToInt32(tempgame)) != null)
                        {
                            dbgames.Add(GameById(Convert.ToInt32(tempgame)));
                        }
                    }
                    if (dbgames.Count > 0)
                    {
                        newcart.Games = dbgames;
                        db.Carts.Add(newcart);
                        await db.SaveChangesAsync();

                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    Cart oldcart = get_unprocessed(customerid);
                    foreach (string tempgame in tempgames)
                    {
                        if (GameById(Convert.ToInt32(tempgame)) != null && !GameExists(Convert.ToInt32(tempgame), customerid))
                        {
                            dbgames.Add(GameById(Convert.ToInt32(tempgame)));
                        }
                    }
                    if(dbgames.Count > 0){
                        oldcart.Games = dbgames;

                        db.Entry(oldcart).State = EntityState.Modified;

                        await db.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Malformed input/Game exists in cart.");
                    }
                }
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost]
        [Route("api/addtocart/{id:int}")]
        // POST api/addtocart/id
        public async Task<IHttpActionResult> AddtoCart(int id)
        {
            if (id > 0)
            {
                ICollection<Game> dbgames = new Collection<Game>(); ;

                string userEmail = HttpContext.Current.User.Identity.Name;
                int customerid = IdFromEmail(userEmail);

                //User has unprocessed cart
                if (!has_oldcart(customerid))
                {
                    //New Cart
                    Cart newcart = new Cart();
                    newcart.isProcessed = false;
                    newcart.CustomerId = customerid;

                    //Add Game
                    if (GameById(Convert.ToInt32(id)) != null)
                    {
                            dbgames.Add(GameById(Convert.ToInt32(id)));
                    }
                    if (dbgames.Count > 0)
                    {
                        newcart.Games = dbgames;
                        db.Carts.Add(newcart);
                        await db.SaveChangesAsync();

                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Empty Cart");
                    }
                }
                else
                {
                    Cart oldcart = get_unprocessed(customerid);
                    if (GameById(Convert.ToInt32(id)) != null && !GameExists(Convert.ToInt32(id),customerid))
                    {
                            dbgames.Add(GameById(Convert.ToInt32(id)));
                    }
                    if (dbgames.Count > 0)
                    {
                        oldcart.Games = dbgames;

                        db.Entry(oldcart).State = EntityState.Modified;

                        await db.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Game already Exists in Cart");
                    }
                }
            }
            return BadRequest();
        }

        public bool GameExists(int gameid, int userid)
        {
            var cart = db.Carts.FirstOrDefault(u => u.CustomerId == userid && u.isProcessed == false);
            if(cart != null){
                if(cart.Games.Any(u => u.Id == gameid)){
                    return true;
                }
            }
            return false;
        }

        public Cart get_unprocessed(int customerid)
        {
            var cart = db.Carts.FirstOrDefault(u => u.CustomerId == customerid && u.isProcessed == false);
            return cart;
        }

        public Cart cart_by_id(int cart_id)
        {
            var cart = db.Carts.FirstOrDefault(u => u.Id == cart_id);
            if(cart != null){
                return cart;
            }
            return null;
        }

        public Game GameById(int id)
        {
            var game = db.Games.FirstOrDefault(u => u.Id == id);
            if(game != null){
                return game;
            }
            return null;
        }

        public bool has_oldcart(int customerid)
        {
            var carts = db.Carts.Where(u => u.CustomerId == customerid && u.isProcessed == false);
            if(carts == null){
                return false;
            }
            return true;
        }

        public int IdFromEmail(string Email)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == Email);
            
            if(user != null){
                return user.Id;
            }
            return -1;
        }

        // GET: api/Cart/5
        [ResponseType(typeof(Cart))]
        public async Task<IHttpActionResult> GetCart(int id)
        {
            Cart cart = await db.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        [Authorize]
        [HttpGet]
        [Route("api/cart/process/{id:int}")]
        public async Task<IHttpActionResult> process(int id)
        {
            string employeeEmail = HttpContext.Current.User.Identity.Name;
            int employeeid = IdFromEmail(employeeEmail);
            
            //Check Valid Cart
            if(cart_by_id(id) != null && cart_by_id(id).isProcessed != true)
            {
                Cart toprocess = cart_by_id(id);

                Sale newsale = new Sale();
                newsale.CartId = id;
                newsale.EmployeeId = employeeid;
                newsale.Date = DateTime.UtcNow;
                newsale.TotalAmount = TotalPrice(toprocess.Games);

                //Process Cart
                toprocess.isProcessed = true;
                db.SaveChanges();

                //Decrease Quantity
                if (DecreaseGameCount(toprocess.Games))
                {
                    //db.Entry(toprocess).State = EntityState.Modified;
                    //db.SaveChanges();

                    //Add to Sale
                    db.Sales.Add(newsale);
                    db.SaveChanges();
                }
            }
                
                
            return Ok();
        }

        public bool DecreaseGameCount(ICollection<Game> Games)
        {
            foreach(Game game in Games){
                game.InventoryCount = game.InventoryCount - 1;
            }
            //Decrease the count
            db.SaveChanges();
            return true;
        }

        // PUT: api/Cart/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCart(int id, Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cart.Id)
            {
                return BadRequest();
            }

            db.Entry(cart).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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

        //// POST: api/Cart
        //[ResponseType(typeof(Cart))]
        //public async Task<IHttpActionResult> PostCart(Cart cart)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Carts.Add(cart);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = cart.Id }, cart);
        //}

        // DELETE: api/Cart/5
        [ResponseType(typeof(Cart))]
        public async Task<IHttpActionResult> DeleteCart(int id)
        {
            Cart cart = await db.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            db.Carts.Remove(cart);
            await db.SaveChangesAsync();

            return Ok(cart);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CartExists(int id)
        {
            return db.Carts.Count(e => e.Id == id) > 0;
        }
    }
}