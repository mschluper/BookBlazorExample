using BookBlazorExample.Services;
using BookBlazorExample.States;
using Microsoft.AspNetCore.Components;

namespace BookBlazorExample.Pages
{
    public partial class ProductList
    {
        private ProductListState state = new();

        [Inject]
        public IProductService? ProductService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            state.Products = await ProductService!.GetProducts();
        }

        private async Task HandleFilterChange(string filterText)
        {
            state.FilterText = filterText;
            await InvokeAsync(StateHasChanged);
        }

        private async Task HandleInStockChange(bool inStockOnly)
        {
            state.InStockOnly = inStockOnly;
            await InvokeAsync(StateHasChanged);
        }
    }
}