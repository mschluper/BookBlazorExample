using BookBlazorExample.Models;

namespace BookBlazorExample.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProducts();
    }

    public class ProductService : IProductService
    {
        public async Task<List<Product>> GetProducts()
        {
            // 1: Electronics
            // 2: Sporting Goods

            List<Product> result = new();
            await Task.Run(() =>
            {
                result = new()
                {
                    new() { Id = 1, CategoryId = 2, Price = 49.99M, IsInStock = true, Name = "Football", Category = new ProductCategory() { Id = 2, Name = "Sporting Goods", } },
                    new() { Id = 2, CategoryId = 2, Price = 9.99M, IsInStock = true, Name = "Baseball", Category = new ProductCategory() { Id = 2, Name = "Sporting Goods", } },
                    new() { Id = 3, CategoryId = 2, Price = 29.99M, IsInStock = false, Name = "Basketball", Category = new ProductCategory() { Id = 2, Name = "Sporting Goods", } },
                    new() { Id = 4, CategoryId = 1, Price = 99.99M, IsInStock = true, Name = "iPod Touch", Category = new ProductCategory() { Id = 1, Name = "Electronics", } },
                    new() { Id = 2, CategoryId = 1, Price = 399.99M, IsInStock = false, Name = "iPhone 5", Category = new ProductCategory() { Id = 1, Name = "Electronics", }},
                    new() { Id = 2, CategoryId = 1, Price = 199.99M, IsInStock = true, Name = "Nexus 7", Category = new ProductCategory() { Id = 1, Name = "Electronics", }}
                };
            });
            return result;
        }
    }
}
