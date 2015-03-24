﻿using System;
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
using GameManager.Models;
using GameManager.DAL;
using System.Web;

namespace GameManager.Controllers
{
    public class UserController : ApiController
    {
        private GameManagerContext db = new GameManagerContext();

        // GET api/User
        //[Authorize(Roles="StoreAdmin")]
        public IQueryable<UserDTO> GetUsers()
        {
            var users = from u in db.Users
                        select new UserDTO()
                        {
                            Id = u.Id,
                            Email = u.Email,
                            Role = u.Role
                        };
            string userEmail = HttpContext.Current.User.Identity.Name;
            return users;
        }

        // GET api/User/5
        [ResponseType(typeof(UserDTO))]
        [Authorize(Roles ="StoreAdmin")]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            var user = await db.Users.Include(u => u.Email).Select(u =>
                new UserDTO()
                {
                    Id = u.Id,
                    Email = u.Email,
                    Role = u.Role
                }).SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT api/User/5
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST api/User
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();

            

            var dto = new UserDetailDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                ApiKey = user.ApiKey,
                Role = user.Role
            };

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, dto);
        }

        // DELETE api/User/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}