using BookBlazorExample.Pages;
using Bunit;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BookBlazorExample.Test.Extensions
{
    /// <summary>
    /// Helper functions that make the test code better readable by means of abstraction from UI details
    /// </summary>
    public static class ProductListExtensions
    {
        public static int GetDisplayedProductCount(this IRenderedComponent<ProductList> cut)
        {
            var rows = cut.FindAll("table > tbody > tr:not(.category)").ToList();
            return rows.Count;
        }

        public static void SetInStockOnly(this IRenderedComponent<ProductList> cut, bool inStockOnly)
        {
            var elements = cut.FindAll("input#in-stock"); // some may prefer cut.Find() which throws an exception if there is none
            if (elements.Count > 0)
            {
                elements.First().Change(inStockOnly);
            }
        }

        public static void SetFilterText(this IRenderedComponent<ProductList> cut, string filterText)
        {
            var elements = cut.FindAll("input[type=\"text\"]");
            if (elements.Count > 0)
            {
                elements.First().Input(new ChangeEventArgs() { Value = filterText, });
            }
        }
    }
}
