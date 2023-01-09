using BookBlazorExample.Models;
using BookBlazorExample.Services;
using BookBlazorExample.States;
using Microsoft.AspNetCore.Components;

namespace BookBlazorExample.Pages
{
    public partial class ProductForm
    {
        private ProductFormState state = new();

        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IProductService? ProductService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            // In a real app this would be retrieved from a database:
            state.ProductCategories = new List<ProductCategory>
            {
                new() { Id = 1, Name = "Electronics", },
                new() { Id = 2, Name = "Sporting Goods", },
            };

            Models.Product? product = null;
            
            if (Id == 0)
            {
                product = new Models.Product();
            }
            else
            {
                // simulate retrieval
                product = await ProductService!.GetProduct(Id);
            }

            state.Id = product!.Id;
            state.Name = product!.Name;
            state.Price = product!.Price;
            state.CategoryId = product!.CategoryId;

            var products = await ProductService!.GetProducts();
            if (products != null)
            {
                state.ExistingProductNames = products.Select(p => p.Name).ToList();
            }
        }

        private void OnInputName(ChangeEventArgs e)
        {
            state.Name = e.Value?.ToString()?.Trim() ?? String.Empty;
            Validate("Name");
            DeterminePendingChanges();
        }

        private void OnChangePrice(ChangeEventArgs e)
        {
            if (decimal.TryParse(e.Value?.ToString(), out var price))
            {
                state.Price = price;
                Validate("Price");
                DeterminePendingChanges();
            }
        }

        private void OnChangeCategory(ChangeEventArgs e)
        {
            state.CategoryId = int.Parse(e.Value?.ToString() ?? "0");
            Validate("Category");
            DeterminePendingChanges();
        }

        private void Validate(string? fieldName = null)
        {
            if (fieldName == null || fieldName == "Name")
            {
                state.NameIsMissing = string.IsNullOrWhiteSpace(state.Name);
                if (!state.NameIsMissing)
                {
                    state.NameIsInUse = IsExistingProductName(state.Name);
                    state.NameIsTooLong = state.Name?.Length > 100;
                }
            }
            if (fieldName == null || fieldName == "Price")
            {
                state.PriceIsInvalid = state.Price <= 0;
            }
            if (fieldName == null || fieldName == "Category")
            {
                state.CategoryIsInvalid = state.CategoryId <= 0;
            }

            // state.ProductIsInvalid is recalculated upon request (this means upon rendering)
        }

        private void DeterminePendingChanges()
        {
            state.HasPendingChanges = true; // needs work
        }

        private bool IsExistingProductName(string? productName)
        {
            return state.ExistingProductNames.Contains(productName ?? string.Empty);
        }
    }
}