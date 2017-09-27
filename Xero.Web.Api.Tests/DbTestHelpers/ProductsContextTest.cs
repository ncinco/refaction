using System.Data.Entity;
using System.Threading.Tasks;
using Xero.Persistence;

namespace Xero.Web.Api.Tests.DbTestHelpers
{
    public class ProductsContextTest : IProductsContext
    {
        public ProductsContextTest()
        {
            this.Products = new TestDbSet<Product>();
            this.ProductOptions = new TestDbSet<ProductOption>();
        }

        public DbSet<Product> Products { get; }

        public DbSet<ProductOption> ProductOptions { get; }

        public int SaveChangesCount { get; private set; }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult(SaveChanges());
        }

        public int SaveChanges()
        {
            SaveChangesCount++;
            return 1;
        }
    }
}