using System.Text.Json.Serialization;

namespace BookBlazorExample.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        [JsonPropertyName("stocked")]
        public bool IsInStock { get; set; }

        public ProductCategory? Category { get; set; }
    }
}
