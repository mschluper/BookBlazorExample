using BookBlazorExample.Pages;
using BookBlazorExample.Services;
using BookBlazorExample.Test.Extensions;
using Bunit;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Moq;
using System.Text.Json;
using Xunit.Abstractions;

namespace BookBlazorExample.Test
{

    public class ProductTests : TestContext, IDisposable
    {
        private readonly ITestOutputHelper testOutput;

        public new void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ProductTests(ITestOutputHelper testOutput)
        {
            var authContext = this.AddTestAuthorization();
            authContext.SetAuthorized("Test User");
            this.testOutput = testOutput;

            this.JSInterop.Mode = JSRuntimeMode.Loose;
        }

        #region setup
        private async Task<List<Models.Product>> ReadProducts()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            using FileStream openStream = File.OpenRead(@"..\..\..\testdata.json");
            var products = await JsonSerializer.DeserializeAsync<List<Models.Product>>(openStream, options) ?? new();
            return products;
        }

        private void MockServices(List<Models.Product> products)
        {
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetProducts()).Returns(Task.FromResult(products));
            Services.TryAddScoped(_ => productServiceMock.Object);
        }
        #endregion

        [Fact]
        [Covers("DisplayProducts")]
        public async Task DisplayProducts()
        {
            var products = await ReadProducts();
            MockServices(products);

            var cut = RenderComponent<ProductList>(parameters =>
            {
            });

            // Have a WaitForAssertion after each potential render to ensure rendering is complete
            cut.WaitForAssertion(() => cut.GetDisplayedProductCount().Should().Be(products.Count));

            //// MORE ASSERTIONS HERE TO ENSURE THE TABLE OF PRODUCTS IS RENDERED PROPERLY
        }

        [Fact]
        [Covers("FilterInStock")]
        public async Task FilterInStock()
        {
            var products = await ReadProducts();
            MockServices(products);

            var cut = RenderComponent<ProductList>(parameters =>
            {
            });

            // Have a WaitForAssertion after each potential render to ensure rendering is complete
            cut.WaitForAssertion(() => cut.GetDisplayedProductCount().Should().Be(products.Count));

            cut.SetInStockOnly(true);

            cut.WaitForAssertion(() => cut.GetDisplayedProductCount().Should().Be(products.Count(p => p.IsInStock)));
            //var displayedProducts = cut.GetProductNames();

            cut.SetInStockOnly(false);

            cut.WaitForAssertion(() => cut.GetDisplayedProductCount().Should().Be(products.Count));
        }

        [Fact]
        [Covers("Search")]
        public async Task Search()
        {
            var products = await ReadProducts();
            MockServices(products);

            var cut = RenderComponent<ProductList>(parameters =>
            {
            });

            // Have a WaitForAssertion after each potential render to ensure rendering is complete
            cut.WaitForAssertion(() => cut.GetDisplayedProductCount().Should().Be(products.Count));

            cut.SetFilterText("b");

            cut.WaitForAssertion(() => cut.GetDisplayedProductCount().Should().Be(products.Count(p => p.Name.Contains("b"))));

            //// MORE ASSERTIONS HERE TO ENSURE THE TABLE OF PRODUCTS IS RENDERED PROPERLY
        }
    }
}