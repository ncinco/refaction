using System.Data.Entity;
using System.Threading.Tasks;

namespace Xero.Persistence
{
    public interface IProductsContext
    {
        DbSet<Product> Products { get; }

        DbSet<ProductOption> ProductOptions { get; }

        Task<int> SaveChangesAsync();

        int SaveChanges();
    }
}