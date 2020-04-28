using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Web.Memberships.Areas.Admin.Models;
using Web.Memberships.Entities;
using Web.Memberships.Models;

namespace Web.Memberships.Extensions
{
    public static class ConversionExtensions
    {
        #region Prduct
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
    }
}