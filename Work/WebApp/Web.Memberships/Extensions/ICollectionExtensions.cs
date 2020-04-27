using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Memberships.Extensions
{
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Converts ICollection<T> to IEnumarable<SelectListItem>. We need this to display items in dropdowns in MVC views
        /// </summary>
        /// <typeparam name="T">The ICollection T to display in the dropdown</typeparam>
        /// <param name="items">The ICollection T items to display in the dropdown</param>
        /// <param name="selectedValue">The selectedValue in the dropdown</param>
        /// <returns>IEnumerable of SelectListItem</returns>
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this ICollection<T> items, int selectedValue)
        {
            if (items == null) return null;

            var selectListItems = items.Select(x => new SelectListItem()
            {
                Text = x.GetPropertyValue("Title"),
                Value = x.GetPropertyValue("Id"), 
                Selected = x.GetPropertyValue("Id").Equals(selectedValue.ToString())
            });

            return selectListItems;
        }
    }
}