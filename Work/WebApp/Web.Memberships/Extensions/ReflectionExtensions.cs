using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Memberships.Extensions
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Checks if the property exists in type T
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="item">The T item</param>
        /// <param name="propertyName">The propertyName in T</param>
        /// <returns>bool</returns>
        public static bool IsPropertyExists<T>(this T item, string propertyName)
        {
            //must have the basics
            if (string.IsNullOrWhiteSpace(propertyName) || item == null) return false;

            //property name exists
            var property = item.GetType().GetProperty(propertyName);
            if (property == null) return false;

            return true;
        }

        /// <summary>
        /// Extension method to get the property value from T using reflections
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="item">The T item</param>
        /// <param name="propertyName">The propertyName in T whose value needs to be fetched</param>
        /// <returns>string</returns>
        public static string GetPropertyValue<T>(this T item, string propertyName)
        {
            //must have the basics
            if (!item.IsPropertyExists(propertyName)) return "";

            var value = item.GetType()
                            .GetProperty(propertyName)
                            .GetValue(item, null)
                            .ToString();
            return value; 
        }
    }
}