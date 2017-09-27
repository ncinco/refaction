using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Nelibur.ObjectMapper;
using Xero.Persistence;
using Xero.Web.Api.Models;
using Product = Xero.Web.Api.Models.Product;
using ProductOption = Xero.Web.Api.Models.ProductOption;

namespace Xero.Web.Api.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        #region Variable Declarations
        private readonly IProductsContext _productsContext;
        #endregion

        #region Constructors
        public ProductsController(IProductsContext productsContext)
        {
            _productsContext = productsContext;
        }
        #endregion

        [Route]
        [HttpGet]
        public async Task<Products> GetAll()
        {
            var products = new Products();

            // query
            var list = await _productsContext.Products.ToListAsync();

            // map
            list.ForEach(p =>
            {
                products.Items.Add(TinyMapper.Map<Product>(p));
            });

            return products;
        }

        [Route]
        [HttpGet]
        public async Task<Products> SearchByName(string name)
        {
            var products = new Products();

            var list = await _productsContext.Products
                .Where(p => p.Name.Contains(name))
                .ToListAsync();

            list.ForEach(p =>
            {
                products.Items.Add(TinyMapper.Map<Product>(p));
            });

            return products;
        }

        [Route("{id:guid}")]
        [HttpGet]
        public async Task<Product> GetProduct(Guid id)
        {
            var product = await _productsContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            // throw not found
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            // return mapped product
            return TinyMapper.Map<Product>(product);
        }

        [Route]
        [HttpPost]
        public async Task Create(Product product)
        {
            var newProduct = TinyMapper.Map<Persistence.Product>(product);
            newProduct.Id = Guid.NewGuid();
            _productsContext.Products.Add(newProduct);
            await _productsContext.SaveChangesAsync();
        }

        [Route("{id:guid}")]
        [HttpPut]
        public async Task Update(Guid id, Product product)
        {
            var prod = await _productsContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            // throw not found
            if (prod == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            // flag for update
            TinyMapper.Map(product, prod);

            await _productsContext.SaveChangesAsync();
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public async Task Delete(Guid id)
        {
            var prod = await _productsContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (prod == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            // remove options
            prod.ProductOptions.ToList().ForEach(opt =>
            {
                _productsContext.ProductOptions.Remove(opt);
            });
            
            // remove product
            _productsContext.Products.Remove(prod);

            await _productsContext.SaveChangesAsync();
        }

        [Route("{productId:guid}/options")]
        [HttpGet]
        public async Task<ProductOptions> GetOptions(Guid productId)
        {
            var options = new ProductOptions();

            var product = await _productsContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var opt = product.ProductOptions
                .Where(p => p.ProductId == productId).ToList();

            // throw if not options found
            if (!opt.Any())
                throw new HttpResponseException(HttpStatusCode.NotFound);

            opt.ForEach(o =>
            {
                options.Items.Add(TinyMapper.Map<ProductOption>(o));
            });

            return options;
        }

        [Route("{productId:guid}/options/{id:guid}")]
        [HttpGet]
        public async Task<ProductOption> GetOption(Guid productId, Guid id)
        {
            var product = await _productsContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            // check product Id and option Id
            var opt = product.ProductOptions
                .FirstOrDefault(p => p.Id == id);

            if (opt == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return TinyMapper.Map<ProductOption>(opt);
        }

        [Route("{productId:guid}/options")]
        [HttpPost]
        public async Task CreateOption(Guid productId, ProductOption option)
        {
            var prod = await _productsContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

            // check if product exists before attempt to add new option
            if (prod == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            
            var newOption = TinyMapper.Map<Persistence.ProductOption>(option);
            newOption.Id = Guid.NewGuid();
            prod.ProductOptions.Add(newOption);

            await _productsContext.SaveChangesAsync();
        }

        [Route("{productId:guid}/options/{id:guid}")]
        [HttpPut]
        public async Task UpdateOption(Guid id, ProductOption option)
        {
            var opt = await _productsContext.ProductOptions.FirstOrDefaultAsync(o => o.Id == id);

            // check if option exists before attempt to update
            if (opt == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            // update
            TinyMapper.Map(option, opt);

            // save
            await _productsContext.SaveChangesAsync();
        }

        [Route("{productId:guid}/options/{id:guid}")]
        [HttpDelete]
        public async Task DeleteOption(Guid id)
        {
            var opt = await _productsContext.ProductOptions.FirstOrDefaultAsync(o => o.Id == id);

            // check if option exists before attempt to update
            if (opt == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var prod = await _productsContext.Products.FirstOrDefaultAsync(p => p.Id == opt.ProductId);

            // check if product exists before attempt to add new option
            if (prod == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            // hack for unit test
            var entry = _productsContext.Entry(opt);
            if(entry != null)
                entry.State = EntityState.Deleted;

            // save
            prod.ProductOptions.Remove(opt);
            await _productsContext.SaveChangesAsync();
        }
    }
}