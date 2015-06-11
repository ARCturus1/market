using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using MobileStore.Domain.DataLayer;
using MobileStore.Domain.Entities;
using Newtonsoft.Json;
using File = System.IO.File;

namespace Market.WebUI.Controllers
{
    [RoutePrefix("api/slider")]
    public class SliderItemsController : ApiController
    {
        private MarketDbContext db = new MarketDbContext();

        [Route("")]
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
        [Authorize(Users = "admin")]
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
        //[ResponseType(typeof(SliderItem))]
        //public async Task<IHttpActionResult> PostSliderItem(SliderItem sliderItem)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.SliderItems.Add(sliderItem);
        //    await db.SaveChangesAsync();

        //    //return CreatedAtRoute("DefaultApi", new { id = sliderItem.Id }, sliderItem);
        //    return Ok(sliderItem);
        //}

        [Route("add")]
        [Authorize(Users = "admin")]
        public async Task<IHttpActionResult> PostAddSlider()
        {
            //if (!Request.Content.IsMimeMultipartContent())
            //{
            //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            //}

            //string root = HttpContext.Current.Server.MapPath("~/Content/Images");
            //var provider = new MultipartFormDataStreamProvider(root);
            //try
            //{
            //    // Read the form data.
            //    await Request.Content.ReadAsMultipartAsync(provider);
                
            //    string data = provider.FormData["data"];
            //    // This illustrates how to get the file names.
            //    foreach (MultipartFileData file in provider.FileData)
            //    {
            //        ;
            //        db.SliderItems.Add(new SliderItem()
            //        {
            //            FilePath =
            //                HttpContext.Current.Request.Url.MakeRelative(new Uri(file.LocalFileName))
            //                    .Remove(0,
            //                        HttpContext.Current.Request.Url.MakeRelative(
            //                            new Uri(HttpContext.Current.Server.MapPath("~"))).Length - 1) +
            //                Path.GetExtension(file.Headers.ContentDisposition.FileName.Split("\"\"".ToCharArray(), StringSplitOptions.None)[1]),
            //            Name = data
            //        });
            //    }
            //    await db.SaveChangesAsync();
            //    return Ok();
            //}
            //catch (System.Exception e)
            //{
            //    return StatusCode(HttpStatusCode.InternalServerError);
            //}

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                HttpFileCollection files = HttpContext.Current.Request.Files;
                var text = HttpContext.Current.Request.Form["data"];
                string fname = String.Empty;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];

                    fname = "Content/Images/" + file.FileName.Trim();
                    file.SaveAs(HttpContext.Current.Server.MapPath("~/" + fname));
                }
                db.SliderItems.Add(new SliderItem() {Name = text, FilePath = fname});
                await db.SaveChangesAsync();
            }
            //HttpContext.Current.Response.ContentType = "text/plain";
            //HttpContext.Current.Response.Write("File/s uploaded successfully!");
            return Ok();
        }

            // DELETE: api/SliderItems/5
        [Authorize(Users = "admin")]
        [ResponseType(typeof(SliderItem))]
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteSliderItem(int id)
        {
            SliderItem sliderItem = await db.SliderItems.FindAsync(id);
            if (sliderItem == null)
            {
                return NotFound();
            }
            var path = HttpContext.Current.Server.MapPath("~/" + sliderItem.FilePath);
            if (File.Exists(path)) File.Delete(path);
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