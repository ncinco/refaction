using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Xero.Persistence
{
    public interface IProductsContext
    {
        DbSet<Product> Products { get; }

        DbSet<ProductOption> ProductOptions { get; }

        Task<int> SaveChangesAsync();

        DbEntityEntry Entry(object entity);

        int SaveChanges();
    }
}