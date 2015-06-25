using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MobileStore.Domain.DataLayer;
using MobileStore.Domain.Entities;

namespace Market.WebUI.Controllers
{
    [RoutePrefix("api/news")]
    public class PostedNewsController : ApiController
    {
        private MarketDbContext db = new MarketDbContext();

        // GET: api/PostedNews
        [Route("")]
        public IQueryable<PostedNew> GetPostedNews()
        {
            return db.PostedNews.OrderBy(p => p.Date);
        }

        // GET: api/PostedNews/5
        [ResponseType(typeof(PostedNew))]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetPostedNew(int id)
        {
            PostedNew postedNew = await db.PostedNews.SingleOrDefaultAsync(p => p.NewId == id);
            //postedNew.Description
            if (postedNew == null)
            {
                return NotFound();
            }

            return Ok(postedNew);
        }

        // PUT: api/PostedNews/5
        [Authorize(Users = "admin")]
        [Route("putnew/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPostedNew(int id, PostedNew postedNew)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != postedNew.NewId)
            {
                return BadRequest();
            }
            postedNew.Date = DateTime.UtcNow;
            db.Entry(postedNew).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostedNewExists(id))
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

        // POST: api/PostedNews
        [Authorize(Users = "admin")]
        [Route("addnew")]
        [ResponseType(typeof(PostedNew))]
        public async Task<IHttpActionResult> PostPostedNew(PostedNew postedNew)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            postedNew.Date = DateTime.UtcNow;
            db.PostedNews.Add(postedNew);
            await db.SaveChangesAsync();

            //return CreatedAtRoute("DefaultApi", new { id = postedNew.NewId }, postedNew);
            return Ok(postedNew);
        }

        // DELETE: api/PostedNews/5
        [Authorize(Users = "admin")]
        [Route("deletenew/{id}")]
        [ResponseType(typeof(PostedNew))]
        public async Task<IHttpActionResult> DeletePostedNew(int id)
        {
            PostedNew postedNew = await db.PostedNews.FindAsync(id);
            if (postedNew == null)
            {
                return NotFound();
            }

            db.PostedNews.Remove(postedNew);
            await db.SaveChangesAsync();

            return Ok(postedNew);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostedNewExists(int id)
        {
            return db.PostedNews.Count(e => e.NewId == id) > 0;
        }
    }
}