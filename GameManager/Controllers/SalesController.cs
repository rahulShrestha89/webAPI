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

namespace GameManager.Controllers
{
    public class SalesController : ApiController
    {
        private GameManagerContext db = new GameManagerContext();

        // GET: api/Sales
        public async Task<IHttpActionResult> GET()
        {
            var sales = db.Sales.Select(u =>
                    new ViewSalesAdmin
                    {
                        Id = u.Id,
                        Date=u.Date,
                        SaleProcessedBy=u.EmployeeId,
                        TotalAmount=u.TotalAmount,
                        CartID = u.CartId
                    }
                ).ToList();

            foreach(ViewSalesAdmin sale in sales){
                sale.Games = GamesFromCartId(sale.CartID);
            }

            if(sales.Count > 0){
                return Ok(sales);
            }
            else
            {
                return NotFound();
            }
        }


        public ICollection<Game> GamesFromCartId(int cartid)
        {
            var cart = db.Carts.FirstOrDefault(u => u.Id == cartid);
            if(cart != null){
                return cart.Games;
            }
            return null;
        }


        // GET: api/Sales/5
        [HttpGet]
        [ResponseType(typeof(Sale))]
        public async Task<IHttpActionResult> GET(int saleid)
        {
            Sale sale = await db.Sales.FindAsync(saleid);
            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetSales(int customerid)
        {
            var carts = db.Carts.Where(u => u.CustomerId == customerid && u.isProcessed == true).Select(u =>
                new ViewSalesCustomer
                {
                    Games = u.Games,
                    CartId = u.Id
                }
            ).ToList();
            
            foreach(ViewSalesCustomer cart in carts){
                cart.TotalAmount = CartController.TotalPrice(cart.Games);
            }

            if (carts != null)
            {
                return Ok(carts);
            }
            return BadRequest();
        }

        public DateTime SalesDateFromCartId(int cartid)
        {
            var date = db.Sales.SingleOrDefault(u => u.CartId == cartid).Date;
            return date;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GET(string name)
        {
            var employeeid = new CartController().IdFromEmail(name);
            var sales = await db.Sales.Where(u => u.EmployeeId == employeeid).ToListAsync();

            if (sales != null)
            {
                return Ok(sales);
            }
            return BadRequest();
        }

        // PUT: api/Sales/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSale(int id, Sale sale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sale.Id)
            {
                return BadRequest();
            }

            db.Entry(sale).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExists(id))
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

        // POST: api/Sales
        [ResponseType(typeof(Sale))]
        public async Task<IHttpActionResult> PostSale(Sale sale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sales.Add(sale);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = sale.Id }, sale);
        }

        // DELETE: api/Sales/5
        [ResponseType(typeof(Sale))]
        public async Task<IHttpActionResult> DeleteSale(int id)
        {
            Sale sale = await db.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            db.Sales.Remove(sale);
            await db.SaveChangesAsync();

            return Ok(sale);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SaleExists(int id)
        {
            return db.Sales.Count(e => e.Id == id) > 0;
        }
    }
}