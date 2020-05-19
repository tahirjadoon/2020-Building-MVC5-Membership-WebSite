using System;

namespace Web.Memberships.Models
{
    public class ProductItemRow
    {
        public int ItemId { get; set; }

        public int ProductId { get; set; }

        public string ImageUrl { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsDownload { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime? ReleaseDate { get; set; }
    }
}