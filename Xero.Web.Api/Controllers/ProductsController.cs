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
        [Route]
        [HttpGet]
        public async Task<Products> GetAll()
        {
            var products = new Products();

            using (var ctx = new ProductsContext())
            {
                // query
                var list = await ctx.Products.ToListAsync();

                // map
                list.ForEach(p =>
                {
                    products.Items.Add(TinyMapper.Map<Product>(p));
                });
            }

            return products;
        }

        [Route]
        [HttpGet]
        public async Task<Products> SearchByName(string name)
        {
            var products = new Products();

            using (var ctx = new ProductsContext())
            {
                var list = await ctx.Products
                    .Where(p => p.Name.Contains(name))
                    .ToListAsync();

                list.ForEach(p =>
                {
                    products.Items.Add(TinyMapper.Map<Product>(p));
                });
            }

            return products;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<Product> GetProduct(Guid id)
        {
            using (var ctx = new ProductsContext())
            {
                var product = await ctx.Products.FirstOrDefaultAsync(p => p.Id == id);

                // throw not found
                if (product == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                // return mapped product
                return TinyMapper.Map<Product>(product);
            }
        }

        [Route]
        [HttpPost]
        public async Task Create(Product product)
        {
            using (var ctx = new ProductsContext())
            {
                // check if flag IsNew
                if (product.IsNew)
                    ctx.Products.Add(TinyMapper.Map<Xero.Persistence.Product>(product));
                else
                    await Update(product.Id, product);

                await ctx.SaveChangesAsync();
            }
        }

        [Route("{id}")]
        [HttpPut]
        public async Task Update(Guid id, Product product)
        {
            using (var ctx = new ProductsContext())
            {
                var prod = await ctx.Products.FirstOrDefaultAsync(p => p.Id == id);

                // throw not found
                if (prod == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                // flag for update
                TinyMapper.Map(product, prod);
                ctx.Entry(prod).State = EntityState.Modified;

                await ctx.SaveChangesAsync();
            }
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task Delete(Guid id)
        {
            using (var ctx = new ProductsContext())
            {
                var prod = await ctx.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (prod == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                // flag for deletion
                ctx.Entry(prod).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
        }

        [Route("{productId}/options")]
        [HttpGet]
        public async Task<ProductOptions> GetOptions(Guid productId)
        {
            var options = new ProductOptions();

            using (var ctx = new ProductsContext())
            {
                var opt = await ctx.ProductOptions
                    .Where(p => p.ProductId == productId)
                    .ToListAsync();

                // throw if not options found
                if (!opt.Any())
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                opt.ForEach(o =>
                {
                    options.Items.Add(TinyMapper.Map<ProductOption>(o));
                });
            }

            return options;
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public async Task<ProductOption> GetOption(Guid productId, Guid id)
        {
            using (var ctx = new ProductsContext())
            {
                // check product Id and option Id
                var opt = await ctx.ProductOptions
                    .FirstOrDefaultAsync(p => p.ProductId == productId && p.Id == id);

                if (opt == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                return TinyMapper.Map<ProductOption>(opt);
            }
        }

        [Route("{productId}/options")]
        [HttpPost]
        public async Task CreateOption(Guid productId, ProductOption option)
        {
            using (var ctx = new ProductsContext())
            {
                var prod = await ctx.Products.FirstOrDefaultAsync(p => p.Id == productId);

                // check if product exists before attempt to add new option
                if (prod == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                // check if flag IsNew
                if (option.IsNew)
                {
                    option.ProductId = productId;
                    ctx.ProductOptions.Add(TinyMapper.Map<Persistence.ProductOption>(option));
                }
                else
                    await UpdateOption(option.Id, option);

                await ctx.SaveChangesAsync();
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public async Task UpdateOption(Guid id, ProductOption option)
        {
            using (var ctx = new ProductsContext())
            {
                var opt = await ctx.ProductOptions.FirstOrDefaultAsync(o => o.Id == id);

                // check if option exists before attempt to update
                if (opt == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                // update
                TinyMapper.Map(option, opt);

                // flag for update
                ctx.Entry(opt).State = EntityState.Modified;

                // save
                await ctx.SaveChangesAsync();
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public async Task DeleteOption(Guid id)
        {
            using (var ctx = new ProductsContext())
            {
                var opt = await ctx.ProductOptions.FirstOrDefaultAsync(o => o.Id == id);

                // check if option exists before attempt to update
                if (opt == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                // flag for deletion
                ctx.Entry(opt).State = EntityState.Deleted;

                // save
                await ctx.SaveChangesAsync();
            }
        }
    }
}