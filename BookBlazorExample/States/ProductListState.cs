namespace BookBlazorExample.States
{
    /// <summary>
    /// Represents the state of a Product List
    /// </summary>
    public class ProductListState
    {
        /// <summary>
        /// The products displayed in the list
        /// </summary>
        public List<Models.Product> Products { get; set; } = new();

        /// <summary>
        /// Whether the products in the list are filtered to show only those that are in stock
        /// </summary>
        public bool InStockOnly { get; set; }

        /// <summary>
        /// Whether the products in the list are filtered to show only those whose names contains FilterText
        /// </summary>
        public string? FilterText { get; set; }
    }
}
