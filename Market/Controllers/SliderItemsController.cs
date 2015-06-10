using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using MobileStore.Domain.DataLayer;
using MobileStore.Domain.Entities;

namespace Market.WebUI.Controllers
{
    [RoutePrefix("api/slider")]
    public class SliderItemsController : ApiController
    {
        private MarketDbContext db = new MarketDbContext();

        // GET: api/SliderItems
        public IQueryable<SliderItem> GetSliderItems()
        {
            return db.SliderItems;
        }

        // GET: api/SliderItems/5
        [ResponseType(typeof(SliderItem))]
        public async Task<IHttpActionResult> GetSliderItem(int id)
        {
            SliderItem sliderItem = await db.SliderItems.FindAsync(id);
            if (sliderItem == null)
            {
                return NotFound();
            }

            return Ok(sliderItem);
        }

        // PUT: api/SliderItems/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSliderItem(int id, SliderItem sliderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sliderItem.Id)
            {
                return BadRequest();
            }

            db.Entry(sliderItem).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SliderItemExists(id))
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

        // POST: api/SliderItems
        [ResponseType(typeof(SliderItem))]
        public async Task<IHttpActionResult> PostSliderItem(SliderItem sliderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SliderItems.Add(sliderItem);
            await db.SaveChangesAsync();

            //return CreatedAtRoute("DefaultApi", new { id = sliderItem.Id }, sliderItem);
            return Ok(sliderItem);
        }

        [Route("add")]
        public async Task<IHttpActionResult> PostAddSlider()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                }
                return Ok();
            }
            catch (System.Exception e)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

            // DELETE: api/SliderItems/5
        [ResponseType(typeof(SliderItem))]
        public async Task<IHttpActionResult> DeleteSliderItem(int id)
        {
            SliderItem sliderItem = await db.SliderItems.FindAsync(id);
            if (sliderItem == null)
            {
                return NotFound();
            }

            db.SliderItems.Remove(sliderItem);
            await db.SaveChangesAsync();

            return Ok(sliderItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SliderItemExists(int id)
        {
            return db.SliderItems.Count(e => e.Id == id) > 0;
        }
    }
}