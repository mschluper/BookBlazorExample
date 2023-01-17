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

    public class ProductFormTests : TestContext, IDisposable
    {
        private readonly ITestOutputHelper testOutput;

        public new void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ProductFormTests(ITestOutputHelper testOutput)
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

        private void MockServices(List<Models.Product> products, int productId = 0)
        {
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetProducts()).Returns(Task.FromResult(products));
            productServiceMock.Setup(x => x.GetProduct(It.IsAny<int>())).Returns(Task.FromResult(products.Find(p => p.Id == productId) ?? new Models.Product()));
            Services.TryAddScoped(_ => productServiceMock.Object);
        }
        #endregion

        [Fact]
        [Covers("ProductNameRequired")]
        public async Task ProductNameRequired()
        {
            var products = await ReadProducts();
            MockServices(products, 1);

            var cut = RenderComponent<ProductForm>(parameters =>
            {
                parameters.Add(p => p.Id, 1);
            });

            // Have a WaitForAssertion after each potential render to ensure rendering is complete
            cut.WaitForAssertion(() => cut.HasEnabledElements("product-name").Should().BeTrue());
            cut.GetProductName().Should().Be("Football");
            cut.HasVisibleValidationErrorMessage(States.ProductFormState.NameIsRequired).Should().BeFalse();

            cut.SetProductName("");

            cut.WaitForAssertion(() => cut.HasVisibleValidationErrorMessage(States.ProductFormState.NameIsRequired).Should().BeTrue());
        }

        [Fact]
        [Covers("ProductNameMaxLength")]
        public async Task ProductNameMaxLength()
        {
            var products = await ReadProducts();
            MockServices(products, 1);

            var cut = RenderComponent<ProductForm>(parameters =>
            {
                parameters.Add(p => p.Id, 1);
            });

            // Have a WaitForAssertion after each potential render to ensure rendering is complete
            cut.WaitForAssertion(() => cut.HasEnabledElements("product-name").Should().BeTrue());

            var maxString = new string('*', 100);
            cut.SetProductName(maxString);
            cut.WaitForAssertion(() => cut.HasVisibleValidationErrorMessage(States.ProductFormState.NameHasMaxLength).Should().BeFalse());
            cut.SetProductName(maxString + "!");
            cut.WaitForAssertion(() => cut.HasVisibleValidationErrorMessage(States.ProductFormState.NameHasMaxLength).Should().BeTrue());
            cut.SetProductName(maxString);
            cut.WaitForAssertion(() => cut.HasVisibleValidationErrorMessage(States.ProductFormState.NameHasMaxLength).Should().BeFalse());
        }

        [Fact]
        [Covers("ProductNameUnique")]
        public async Task ProductNameUnique()
        {
            var products = await ReadProducts();
            MockServices(products, 1);

            var cut = RenderComponent<ProductForm>(parameters =>
            {
                parameters.Add(p => p.Id, 1);
            });

            // Have a WaitForAssertion after each potential render to ensure rendering is complete
            cut.WaitForAssertion(() => cut.HasEnabledElements("product-name").Should().BeTrue());
            cut.WaitForAssertion(() => cut.HasVisibleValidationErrorMessage(States.ProductFormState.NameNotUnique).Should().BeFalse());

            cut.SetProductName("Baseball");
            cut.WaitForAssertion(() => cut.HasVisibleValidationErrorMessage(States.ProductFormState.NameNotUnique).Should().BeTrue());
            cut.SetProductName("Whatever unique string");
            cut.WaitForAssertion(() => cut.HasVisibleValidationErrorMessage(States.ProductFormState.NameNotUnique).Should().BeFalse());
        }

        [Fact]
        //[Covers("ProductPriceRequired")]
        public async Task ProductPriceRequired()
        {
            var products = await ReadProducts();
            MockServices(products, 1);

            var cut = RenderComponent<ProductForm>(parameters =>
            {
                parameters.Add(p => p.Id, 1);
            });
        }

        [Fact]
        //[Covers("ProductCategoryRequired")]
        public async Task ProductCategoryRequired()
        {
            var products = await ReadProducts();
            MockServices(products, 1);

            var cut = RenderComponent<ProductForm>(parameters =>
            {
                parameters.Add(p => p.Id, 1);
            });
        }
    }
}