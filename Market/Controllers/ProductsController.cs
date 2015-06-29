using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using MobileStore.Domain.DataLayer;
using MobileStore.Domain.Entities;

namespace Market.WebUI.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private MarketDbContext db = new MarketDbContext();

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            return db.Products.Include(x => x.Images);
        }

        [Route("byCategory/{category}")]
        public IQueryable<Product> GetProducts(string category)
        {
            if (category != null)
                return db.Products.Include(x => x.Images).Where(p => p.Category == category);
            return db.Products;
        }

        // GET: api/Products/5
        [Route("product/{id}")]
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            Product product = await db.Products.Include(x => x.Images).SingleAsync(p => p.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [Route("images/{id}")]
        public async Task<IHttpActionResult> GetImagesForProduct(int id)
        {
            Product product = await db.Products.Include(x => x.Images).SingleAsync(p => p.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product.Images);
        }

        [Route("categories")]
        // GET: api/Products
        public IQueryable<string> GetCategories()
        {
            return db.Products.Select(p => p.Category).Distinct();
        }

        // PUT: api/Products/5
        [Authorize(Users = "admin")]
        [Route("update/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        [Authorize(Users = "admin")]
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        [Authorize(Users = "admin")]
        [Route("addImage/{id}")]
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostAddImageForProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                HttpFileCollection files = HttpContext.Current.Request.Files;

                string fname = String.Empty;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];

                    fname = "Content/Images/" + file.FileName.Trim();
                    file.SaveAs(HttpContext.Current.Server.MapPath("~/" + fname));
                }
                if (product.Images == null) product.Images = new List<File>();

                product.Images.Add(new File() { FileName = fname });
                db.Entry(product).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok(product);
        }

        [Authorize(Users = "admin")]
        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        //'api/products/' + productId + '/image/delete/' + imageId
        [Authorize(Users = "admin")]
        // DELETE: api/Products/5
        [Route("{productId}/image/{imageId}")]
        public async Task<IHttpActionResult> DeleteProductsImage(int productId, int imageId)
        {
            Product product = await db.Products.Include(x => x.Images).SingleOrDefaultAsync(x => x.ProductID == productId);
            if (product == null)
            {
                return NotFound();
            }
            var image = product.Images.SingleOrDefault(x => x.Id == imageId);
            if (image != null)
            {
                var path = HttpContext.Current.Server.MapPath("~/" + image.FileName);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }

            product.Images.Remove(image);

            db.Entry(product).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(productId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(product.Images);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductID == id) > 0;
        }
    }
}