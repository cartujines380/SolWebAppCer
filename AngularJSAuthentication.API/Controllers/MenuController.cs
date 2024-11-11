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
using AngularJSAuthentication.API.Custom;
using AngularJSAuthentication.API.Models;

namespace AngularJSAuthentication.API.Controllers
{
    public class MenuController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Menu
        public IQueryable<Menu> GetMenues(string ClienteId)
        {
            //string ApplicationId = string.Empty;

            //string clientId = string.Empty;
            //string clientSecret = string.Empty;
          

            //clibEncripta.clsEncripta objEnc = new clibEncripta.clsEncripta();
             

            //var DeSecretClientId = objEnc.Desencripta(ClienteId, InitialiseService.Semilla);

           

            return db.Menues;


        }

        // GET: api/Menu/5
        [ResponseType(typeof(Menu))]
        public async Task<IHttpActionResult> GetMenu(int id)
        {
            Menu menu = await db.Menues.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            return Ok(menu);
        }

        // PUT: api/Menu/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMenu(int id, Menu menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != menu.Id)
            {
                return BadRequest();
            }

            db.Entry(menu).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
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

        // POST: api/Menu
        [ResponseType(typeof(Menu))]
        public async Task<IHttpActionResult> PostMenu(Menu menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Menues.Add(menu);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = menu.Id }, menu);
        }

        // DELETE: api/Menu/5
        [ResponseType(typeof(Menu))]
        public async Task<IHttpActionResult> DeleteMenu(int id)
        {
            Menu menu = await db.Menues.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            db.Menues.Remove(menu);
            await db.SaveChangesAsync();

            return Ok(menu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MenuExists(int id)
        {
            return db.Menues.Count(e => e.Id == id) > 0;
        }
    }
}