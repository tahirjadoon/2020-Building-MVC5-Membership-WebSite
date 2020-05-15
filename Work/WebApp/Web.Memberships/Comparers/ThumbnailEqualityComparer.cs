﻿using System.Collections.Generic;
using Web.Memberships.Models;

namespace Web.Memberships.Comparers
{
    public class ThumbnailEqualityComparer : IEqualityComparer<ThumbnailModel>
    {
        public bool Equals(ThumbnailModel thumb1, ThumbnailModel thumb2)
        {
            return thumb1.ProductId.Equals(thumb2.ProductId);
        }

        public int GetHashCode(ThumbnailModel thumb)
        {
            return thumb.ProductId;
        }
    }
}