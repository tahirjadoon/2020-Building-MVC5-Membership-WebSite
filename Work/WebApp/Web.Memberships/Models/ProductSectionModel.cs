using System.Collections.Generic;

namespace Web.Memberships.Models
{
    public class ProductSectionModel
    {
        public string Title { get; set; }

        public List<ProductSection> Sections { get; set; }
    }
}