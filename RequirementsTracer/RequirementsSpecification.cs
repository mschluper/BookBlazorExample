using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequirementsTracer
{
    public static class RequirementsSpecification
    {
        public static List<Requirement> Requirements = new()
        {
            new()
            {
                Id = "App",
                PassedTests = false,
            },
            new()
            {
                Id = "ProductList",
                ParentId = "App",
                PassedTests = false,
            },
            new()
            {
                Id = "DisplayProducts",
                Definition = "Upon opening the page, the products are retrieved and assigned to ProductListState.Products, and a list of filtered products is displayed on the page after having been filtered using the values of ProductListState.IsInStock and ProductListState.FilterText, excluding products whose IsInStock value equals false if ProductListState.IsInStock equals true and excluding products whose name does not include the ProductListState.FilterText.",
                ParentId = "ProductList",
                IsCoveredByTest = false,
                PassedTests = false,
            },
            new()
            {
                Id = "FilterInStock",
                Definition = "Upon changing the value of ProductListState.IsInStock, the displayed list of filtered products is updated using the new filter values and the products held by ProductListState.Products",
                ParentId = "ProductList",
            },
            new()
            {
                Id = "Search",
                Definition = "Upon changing the value of ProductListState.FilterText, the displayed list of filtered products is updated using the new filter values and the products held by ProductListState.Products",
                ParentId = "ProductList",
            },
            new()
            {
                Id = "Reports",
                ParentId = "App",
                PassedTests = false,
            },
            new()
            {
                Id = "RequirementCoverage",
                ParentId = "Reports",
                Definition = "Upon opening the page, the app's requirements are listed, with for each an Id, a definition, whether they have a covering test, and whether the tests passed.",
                PassedTests = false,
            },
        };
    }
}
