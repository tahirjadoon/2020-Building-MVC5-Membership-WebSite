using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Memberships.Entities;
using Web.Memberships.Models;
using Web.Memberships.Extensions;
using Web.Memberships.Areas.Admin.Models;

namespace Web.Memberships.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductItemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ProductItem
        public async Task<ActionResult> Index()
        {
            //passing the iQueryable
            var model = await db.ProductItems.Convert(db);
            return View(model);
        }

        // GET: Admin/ProductItem/Details/5
        public async Task<ActionResult> Details(int? itemId, int? productId)
        {
            if (itemId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productItem = await GetProductItem(itemId, productId);
            if (productItem == null)
            {
                return HttpNotFound();
            }

            return View(await productItem.Convert(db));
        }

        // GET: Admin/ProductItem/Create
        public async Task<ActionResult> Create()
        {
            var model = new ProductItemModel
            {
                Items = await db.Items.ToListAsync(),
                Products = await db.Products.ToListAsync()
            };
            return View(model);
        }

        // POST: Admin/ProductItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductId,ItemId")] ProductItem productItem)
        {
            if (ModelState.IsValid)
            {
                var canCreate = await productItem.CanCreate(db);
                if (canCreate)
                {
                    db.ProductItems.Add(productItem);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Info already created!");
            }
            var model = await productItem.Convert(db);
            return View(model);
        }

        // GET: Admin/ProductItem/Edit/5
        public async Task<ActionResult> Edit(int? itemId, int? productId)
        {
            if (itemId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productItem = await GetProductItem(itemId, productId);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            var model = await productItem.Convert(db);
            return View(model);
        }

        // POST: Admin/ProductItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductId,ItemId,OldProductId,OldItemId")] ProductItem productItem)
        {
            if (ModelState.IsValid)
            {
                var canChange = await productItem.CanChange(db);
                if (canChange)
                    await productItem.Change(db);

                return RedirectToAction("Index");
            }

            var model = await productItem.Convert(db);
            return View(model);
        }

        // GET: Admin/ProductItem/Delete/5
        public async Task<ActionResult> Delete(int? itemId, int? productId)
        {
            if (itemId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productItem = await GetProductItem(itemId, productId);
            if (productItem == null)
            {
                return HttpNotFound();
            }

            var model = await productItem.Convert(db);
            return View(model);
        }

        // POST: Admin/ProductItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int itemId, int productId)
        {
            var productItem = await GetProductItem(itemId, productId);
            db.ProductItems.Remove(productItem);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task<ProductItem> GetProductItem(int? itemId, int? productId)
        {
            try
            {
                int itmId = 0, prdId = 0;
                int.TryParse(itemId.ToString(), out itmId);
                int.TryParse(productId.ToString(), out prdId);
                var productItem = await db.ProductItems.FirstOrDefaultAsync(pi => pi.ProductId.Equals(prdId) && pi.ItemId.Equals(itmId));
                return productItem;
            }
            catch { return null; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
