using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using Web.Memberships.Areas.Admin.Models;
using Web.Memberships.Entities;
using Web.Memberships.Models;

namespace Web.Memberships.Extensions
{
    public static class ConversionExtensions
    {
        #region Product
        public static async Task<IEnumerable<ProductModel>> Convert(this IEnumerable<Product> products, ApplicationDbContext db)
        {
            if (products == null || !products.Any() || db == null)
                return new List<ProductModel>();

            var texts = await db.ProductLinkTexts.ToListAsync();
            var types = await db.ProductTypes.ToListAsync();

            var newProducts = products.Select(p => new ProductModel() 
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                ProductLinkTextId = p.ProductLinkTextId,
                ProductTypeId = p.ProductTypeId,
                ProductLinkTexts = texts,
                ProductTypes = types
            });

            return newProducts;
        }

        public static async Task<ProductModel> Convert(this Product product, ApplicationDbContext db)
        {
            if (product == null || db == null)
                return new ProductModel();

            var text = await db.ProductLinkTexts.FirstOrDefaultAsync(p => p.Id.Equals(product.ProductLinkTextId));
            var type = await db.ProductTypes.FirstOrDefaultAsync(p => p.Id.Equals(product.ProductTypeId));

            var model = new ProductModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                ProductLinkTextId = product.ProductLinkTextId,
                ProductTypeId = product.ProductTypeId,
                ProductLinkTexts = new List<ProductLinkText>(),
                ProductTypes = new List<ProductType>()
            };

            model.ProductLinkTexts.Add(text);
            model.ProductTypes.Add(type);

            return model;
        }

        #endregion

        #region ProductItem

        public static async Task<IEnumerable<ProductItemModel>> Convert(this IQueryable<ProductItem> productItems, ApplicationDbContext db)
        {
            if (productItems == null || !productItems.Any() || db == null)
                return new List<ProductItemModel>();

            var model = await (from pi in productItems
                               select new ProductItemModel
                               {
                                   ItemId = pi.ItemId,
                                   ProductId = pi.ProductId,
                                   ItemTitle = db.Items.FirstOrDefault(i => i.Id.Equals(pi.ItemId)).Title, 
                                   ProductTitle = db.Products.FirstOrDefault(p => p.Id.Equals(pi.ProductId)).Title
                               }).ToListAsync();
            return model;
        }

        public static async Task<ProductItemModel> Convert(this ProductItem productItem, ApplicationDbContext db, bool addListData = true)
        {
            if (productItem == null || db == null)
                return new ProductItemModel();

            var model = new ProductItemModel
            {
                ItemId = productItem.ItemId,
                ProductId = productItem.ProductId,
                Items = addListData ? await db.Items.ToListAsync() : null,
                Products = addListData ? await db.Products.ToListAsync() : null,
                ItemTitle = (await db.Items.FirstOrDefaultAsync(i => i.Id.Equals(productItem.ItemId))).Title,
                ProductTitle = (await db.Products.FirstOrDefaultAsync(p => p.Id.Equals(productItem.ProductId))).Title
            };

            return model;
        }

        public static async Task<bool> CanChange(this ProductItem productItem, ApplicationDbContext db)
        {
            if (productItem == null || db == null)
                return false;

            //check that the current is available
            var oldPI = await db.ProductItems.CountAsync(pi => pi.ProductId.Equals(productItem.OldProductId) && pi.ItemId.Equals(productItem.OldItemId));

            //make sure that the new is not already selected
            var newPI = await db.ProductItems.CountAsync(pi => pi.ProductId.Equals(productItem.ProductId) && pi.ItemId.Equals(productItem.ItemId));

            return oldPI.Equals(1) && newPI.Equals(0);
        }

        public static async Task Change(this ProductItem productItem, ApplicationDbContext db)
        {
            var oldProductItem = await db.ProductItems.FirstOrDefaultAsync(pi => pi.ProductId.Equals(productItem.OldProductId) && pi.ItemId.Equals(productItem.OldItemId));

            var newProductItem = await db.ProductItems.FirstOrDefaultAsync(pi => pi.ProductId.Equals(productItem.ProductId) && pi.ItemId.Equals(productItem.ItemId));

            if (oldProductItem != null && newProductItem == null)
            {
                newProductItem = new ProductItem { ItemId = productItem.ItemId, ProductId = productItem.ProductId };

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        db.ProductItems.Remove(oldProductItem);
                        db.ProductItems.Add(newProductItem);

                        await db.SaveChangesAsync();
                        transaction.Complete();
                    }
                    catch { transaction.Dispose(); }
                }
            }
        }

        public static async Task<bool> CanCreate(this ProductItem productItem, ApplicationDbContext db)
        {
            if (productItem == null || db == null)
                return false;

            //make sure that the new is not already selected
            var newPI = await db.ProductItems.CountAsync(pi => pi.ProductId.Equals(productItem.ProductId) && pi.ItemId.Equals(productItem.ItemId));

            return newPI.Equals(0);
        }

        #endregion

        #region SubscriptionProduct

        public static async Task<IEnumerable<SubscriptionProductModel>> Convert(this IQueryable<SubscriptionProduct> subscriptionProducts, ApplicationDbContext db)
        {
            if (subscriptionProducts == null || !subscriptionProducts.Any() || db == null)
                return new List<SubscriptionProductModel>();

            var model = await (from pi in subscriptionProducts
                          select new SubscriptionProductModel
                          {
                              SubscriptionId = pi.SubscriptionId,
                              ProductId = pi.ProductId,
                              SubscriptionTitle = db.Subscriptions.FirstOrDefault(i => i.Id.Equals(pi.SubscriptionId)).Title,
                              ProductTitle = db.Products.FirstOrDefault(p => p.Id.Equals(pi.ProductId)).Title
                          }).ToListAsync();
            return model;
        }

        public static async Task<SubscriptionProductModel> Convert(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db, bool addListData = true)
        {
            if (subscriptionProduct == null || db == null)
                return new SubscriptionProductModel();

            var model = new SubscriptionProductModel
            {
                SubscriptionId = subscriptionProduct.SubscriptionId,
                ProductId = subscriptionProduct.ProductId,
                Subscriptions = addListData ? await db.Subscriptions.ToListAsync() : null,
                Products = addListData ? await db.Products.ToListAsync() : null,
                SubscriptionTitle = (await db.Subscriptions.FirstOrDefaultAsync(s => s.Id.Equals(subscriptionProduct.SubscriptionId))).Title,
                ProductTitle = (await db.Products.FirstOrDefaultAsync(p => p.Id.Equals(subscriptionProduct.ProductId))).Title
            };

            return model;
        }

        public static async Task<bool> CanChange(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db)
        {
            if (subscriptionProduct == null || db == null)
                return false;

            //check current is available
            var oldSP = await db.SubscriptionProducts.CountAsync(sp => sp.ProductId.Equals(subscriptionProduct.OldProductId) && sp.SubscriptionId.Equals(subscriptionProduct.OldSubscriptionId));

            //make sure that new is not already selected
            var newSP = await db.SubscriptionProducts.CountAsync(sp => sp.ProductId.Equals(subscriptionProduct.ProductId) && sp.SubscriptionId.Equals(subscriptionProduct.SubscriptionId));

            return oldSP.Equals(1) && newSP.Equals(0);
        }

        public static async Task Change(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db)
        {
            var oldSubscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(sp => sp.ProductId.Equals(subscriptionProduct.OldProductId) && sp.SubscriptionId.Equals(subscriptionProduct.OldSubscriptionId));

            var newSubscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(sp => sp.ProductId.Equals(subscriptionProduct.ProductId) && sp.SubscriptionId.Equals(subscriptionProduct.SubscriptionId));

            if (oldSubscriptionProduct != null && newSubscriptionProduct == null)
            {
                newSubscriptionProduct = new SubscriptionProduct { SubscriptionId = subscriptionProduct.SubscriptionId, ProductId = subscriptionProduct.ProductId };

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        db.SubscriptionProducts.Remove(oldSubscriptionProduct);
                        db.SubscriptionProducts.Add(newSubscriptionProduct);

                        await db.SaveChangesAsync();
                        transaction.Complete();
                    }
                    catch { transaction.Dispose(); }
                }
            }
        }

        public static async Task<bool> CanCreate(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db)
        {
            if (subscriptionProduct == null || db == null)
                return false;

            //make sure that the new is not already selected
            var newSP = await db.SubscriptionProducts.CountAsync(sp => sp.ProductId.Equals(subscriptionProduct.ProductId) && sp.SubscriptionId.Equals(subscriptionProduct.SubscriptionId));

            return newSP.Equals(0);
        }

        #endregion
    }
}