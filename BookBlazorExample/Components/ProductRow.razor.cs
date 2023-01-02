using BookBlazorExample.Models;
using Microsoft.AspNetCore.Components;

namespace BookBlazorExample.Components
{
    public partial class ProductRow : IComponent
    {
        [Parameter]
        public Product? Product { get; set; }
    }
}