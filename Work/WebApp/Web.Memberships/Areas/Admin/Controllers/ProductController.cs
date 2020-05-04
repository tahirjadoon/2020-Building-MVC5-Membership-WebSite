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
using System.Transactions;

namespace Web.Memberships.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Product
        public async Task<ActionResult> Index()
        {
            var products = await db.Products.ToListAsync();
            var model = await products.Convert(db);
            return View(model);
        }

        // GET: Admin/Product/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var model = await product.Convert(db);
            return View(model);
        }

        // GET: Admin/Product/Create
        public async Task<ActionResult> Create()
        {
            var model = new ProductModel
            {
                ProductLinkTexts = await db.ProductLinkTexts.ToListAsync(),
                ProductTypes = await db.ProductTypes.ToListAsync()

            };
            return View(model);
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,ImageUrl,ProductLinkTextId,ProductTypeId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //very important to do it as IEnumerable or the dropdown will only show a single item 
            var products = new List<Product>();
            products.Add(product);
            var model = await products.Convert(db);
            return View(model.First());
        }

        // GET: Admin/Product/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            //very important to do it as IEnumerable or the dropdown will only show a single item 
            var products = new List<Product>();
            products.Add(product);
            var model = await products.Convert(db);
            return View(model.First());
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,ImageUrl,ProductLinkTextId,ProductTypeId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            //very important to do it as IEnumerable or the dropdown will only show a single item 
            var products = new List<Product>();
            products.Add(product);
            var model = await products.Convert(db);
            return View(model.First());
        }

        // GET: Admin/Product/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var model = await product.Convert(db);
            return View(model);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var product = await db.Products.FindAsync(id);

            //doing it through transactions 
            //we need to remove the ProductItems and ProductSubscriptions as well. 
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var prodItems = db.ProductItems.Where(pi => pi.ProductId.Equals(id));
                    var prodSubscr = db.SubscriptionProducts.Where(sp => sp.ProductId.Equals(id));

                    db.ProductItems.RemoveRange(prodItems);
                    db.SubscriptionProducts.RemoveRange(prodSubscr);
                    db.Products.Remove(product);

                    await db.SaveChangesAsync();
                    transaction.Complete();
                }
                catch { transaction.Dispose(); }
            }

            return RedirectToAction("Index");
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
