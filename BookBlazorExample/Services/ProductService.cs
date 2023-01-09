using BookBlazorExample.Models;
using System.Xml.Linq;

namespace BookBlazorExample.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProducts();

        Task<Product> GetProduct(int id);
    }

    public class ProductService : IProductService
    {
        private List<Product> products = new ()
        {
            new () { Id = 1, CategoryId = 2, Price = 49.99M, IsInStock = true, Name = "Football", Category = new ProductCategory() { Id = 2, Name = "Sporting Goods", } },
            new () { Id = 2, CategoryId = 2, Price = 9.99M, IsInStock = true, Name = "Baseball", Category = new ProductCategory() { Id = 2, Name = "Sporting Goods", } },
            new() { Id = 3, CategoryId = 2, Price = 29.99M, IsInStock = false, Name = "Basketball", Category = new ProductCategory() { Id = 2, Name = "Sporting Goods", } },
            new() { Id = 4, CategoryId = 1, Price = 99.99M, IsInStock = true, Name = "iPod Touch", Category = new ProductCategory() { Id = 1, Name = "Electronics", } },
            new() { Id = 5, CategoryId = 1, Price = 399.99M, IsInStock = false, Name = "iPhone 5", Category = new ProductCategory() { Id = 1, Name = "Electronics", } },
            new() { Id = 6, CategoryId = 1, Price = 199.99M, IsInStock = true, Name = "Nexus 7", Category = new ProductCategory() { Id = 1, Name = "Electronics", } }
        };

        public async Task<List<Product>> GetProducts()
        {
            // 1: Electronics
            // 2: Sporting Goods

            List<Product> result = new();
            // simulating async behavior
            await Task.Run(() =>
            {
                result = products;
            });
            return result;
        }

        public async Task<Product> GetProduct(int id)
        {
            Models.Product? product = null;
            // simulating async behavior
            await Task.Run(() =>
            {
                product = products.FirstOrDefault(p => p.Id == id);
            });
            if (product != null)
            {
                return product;
            }
            throw new ApplicationException($"There is no product with Id equal to {id}. ");
        }
    }
}
