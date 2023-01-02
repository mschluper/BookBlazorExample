namespace BookBlazorExample.States
{
    public class ProductListState
    {
        public List<Models.Product> Products { get; set; } = new();
        public bool InStockOnly { get; set; }
        public string? FilterText { get; set; }
    }
}
