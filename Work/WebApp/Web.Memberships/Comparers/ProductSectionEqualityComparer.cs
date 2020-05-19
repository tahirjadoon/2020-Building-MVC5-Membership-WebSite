using System.Collections.Generic;
using Web.Memberships.Models;

namespace Web.Memberships.Comparers
{
    public class ProductSectionEqualityComparer : IEqualityComparer<ProductSection>
    {
        public bool Equals(ProductSection section1, ProductSection section2)
        {
            return section1.Id.Equals(section2.Id);
        }

        public int GetHashCode(ProductSection section)
        {
            return (section.Id).GetHashCode();
        }
    }
}