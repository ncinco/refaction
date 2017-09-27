using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xero.Web.Api.Controllers;
using Xero.Web.Api.Models;
using Xero.Web.Api.Tests.DbTestHelpers;

namespace Xero.Web.Api.Tests
{
    [TestClass]
    public class ProductsControllerTests
    {
        #region Properties

        public ProductsContextTest ProductContext { get; set; }

        public ProductsController ProductsController { get; set; }
        #endregion

        #region Initializations
        [TestInitialize]
        public void TestInitialize()
        {
            ProductContext = new ProductsContextTest();
            ProductsController = new ProductsController(ProductContext);

            InitializeData();
        }

        private void InitializeData()
        {
            var product = new Persistence.Product
            {
                Id = Guid.NewGuid(),
                Name = "Samsung",
                Description = "New Samsung",
                Price = 1000,
                DeliveryPrice = 15
            };

            var option1 = new Persistence.ProductOption
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Name = "White",
                Description = "White Samsung",
            };
            var option2 = new Persistence.ProductOption
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Name = "Black",
                Description = "Black Samsung",
            };

            ProductContext.Products.Add(product);
            ProductContext.ProductOptions.Add(option1);
            ProductContext.ProductOptions.Add(option2);
            product.ProductOptions.Add(option1);
            product.ProductOptions.Add(option2);


            product = new Persistence.Product
            {
                Id = Guid.NewGuid(),
                Name = "iPhone",
                Description = "New iPhone",
                Price = 1200,
                DeliveryPrice = 12
            };

            option1 = new Persistence.ProductOption
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Name = "White",
                Description = "White iPhone",
            };
            option2 = new Persistence.ProductOption
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Name = "Black",
                Description = "Black iPhone",
            };

            ProductContext.Products.Add(product);
            ProductContext.ProductOptions.Add(option1);
            ProductContext.ProductOptions.Add(option2);
            product.ProductOptions.Add(option1);
            product.ProductOptions.Add(option2);
        }

        [TestMethod]
        public async Task ProductsController_GetAll()
        {
            var products = await ProductsController.GetAll();

            Assert.IsNotNull(products);
            Assert.AreEqual(2, products.Items.Count);

            Assert.AreEqual("Samsung", products.Items[0].Name);
            Assert.AreEqual("iPhone", products.Items[1].Name);
        }

        [TestMethod]
        public async Task ProductsController_SearchByName()
        {
            var products = await ProductsController.SearchByName("Samsung");

            Assert.IsNotNull(products);
            Assert.AreEqual(1, products.Items.Count);
            Assert.AreEqual("Samsung", products.Items[0].Name);


            products = await ProductsController.SearchByName("iPhone");

            Assert.IsNotNull(products);
            Assert.AreEqual(1, products.Items.Count);
            Assert.AreEqual("iPhone", products.Items[0].Name);
        }

        [TestMethod]
        public async Task ProductsController_GetProduct()
        {
            var products = await ProductsController.SearchByName("Samsung");
            var product = await ProductsController.GetProduct(products.Items[0].Id);

            Assert.IsNotNull(product);
            Assert.AreEqual(products.Items[0].Name, product.Name);

            products = await ProductsController.SearchByName("iPhone");
            product = await ProductsController.GetProduct(products.Items[0].Id);

            Assert.IsNotNull(product);
            Assert.AreEqual(products.Items[0].Name, product.Name);
        }

        #endregion
        [TestMethod]
        public async Task ProductsController_Create()
        {
            var newProduct = new Product
            {
                Name = "Name",
                Description = "Description",
                Price = 1000,
                DeliveryPrice = 10
            };

            await ProductsController.Create(newProduct);
            var dbProducts = await ProductsController.SearchByName(newProduct.Name);
            var dbProduct = dbProducts.Items[0];

            Assert.IsNotNull(dbProduct);
            Assert.AreEqual(newProduct.Name, dbProduct.Name);
            Assert.AreEqual(newProduct.Description, dbProduct.Description);
            Assert.AreEqual(newProduct.Price, dbProduct.Price);
            Assert.AreEqual(newProduct.DeliveryPrice, dbProduct.DeliveryPrice);
        }

        [TestMethod]
        public async Task ProductsController_Update()
        {
            var products = await ProductsController.SearchByName("iPhone");
            var product = await ProductsController.GetProduct(products.Items[0].Id);

            var productId = product.Id;

            product.Name = "Dummy Name";
            product.Description = "Dummy Name";
            product.Price = 500M;
            product.DeliveryPrice = 5M;

            await ProductsController.Update(product.Id, product);

            product = await ProductsController.GetProduct(productId);

            Assert.AreEqual("Dummy Name", product.Name);
            Assert.AreEqual("Dummy Name", product.Description);
            Assert.AreEqual(500M, product.Price);
            Assert.AreEqual(5M, product.DeliveryPrice);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task ProductsController_Delete()
        {
            var products = await ProductsController.SearchByName("iPhone");
            var product = await ProductsController.GetProduct(products.Items[0].Id);

            var productId = product.Id;

            await ProductsController.Delete(productId);

            // exception here
            await ProductsController.GetProduct(productId);
        }

        [TestMethod]
        public async Task ProductsController_GetOptions()
        {
            var products = await ProductsController.SearchByName("Samsung");
            var product = await ProductsController.GetProduct(products.Items[0].Id);

            var productId = product.Id;

            var options = await ProductsController.GetOptions(productId);

            Assert.AreEqual(2, options.Items.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task ProductsController_GetOption()
        {
            var products = await ProductsController.SearchByName("Samsung");
            var product = await ProductsController.GetProduct(products.Items[0].Id);

            var productId = product.Id;

            var options = await ProductsController.GetOptions(productId);
            var option = await ProductsController.GetOption(productId, options.Items[0].Id);

            Assert.IsNotNull(option);

            // exception here
            await ProductsController.GetOptions(Guid.Empty);
        }

        [TestMethod]
        public async Task ProductsController_CreateOption()
        {
            var products = await ProductsController.SearchByName("Samsung");
            var product = await ProductsController.GetProduct(products.Items[0].Id);

            var option = new ProductOption
            {
                ProductId = product.Id,
                Name = "Dummy Name",
                Description = "Dummy Description"
            };

            await ProductsController.CreateOption(product.Id, option);
            var options = await ProductsController.GetOptions(product.Id);

            // one option is named of them is "Dummy"
            Assert.IsTrue(options.Items.Any(opt => opt.Name == option.Name));
        }

        [TestMethod]
        public async Task ProductsController_UpdateOption()
        {
            var products = await ProductsController.SearchByName("Samsung");
            var product = await ProductsController.GetProduct(products.Items[0].Id);

            var option = (await ProductsController.GetOptions(product.Id)).Items[0];

            option.Name = "Dummy Name";
            option.Description = "Dummy Description";

            await ProductsController.UpdateOption(option.Id, option);
            var dbOption = await ProductsController.GetOption(product.Id, option.Id);

            Assert.AreEqual(option.Id, dbOption.Id);
            Assert.AreEqual(option.ProductId, dbOption.ProductId);
            Assert.AreEqual(option.Name, dbOption.Name);
            Assert.AreEqual(option.Description, dbOption.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task ProductsController_DeleteOption()
        {
            var products = await ProductsController.SearchByName("Samsung");
            var product = await ProductsController.GetProduct(products.Items[0].Id);

            var option = (await ProductsController.GetOptions(product.Id)).Items[0];

            await ProductsController.DeleteOption(option.Id);

            // exception here
            await ProductsController.GetOption(product.Id, option.Id);
        }
    }
}