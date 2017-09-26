using System.Collections.Generic;

namespace Xero.Web.Api.Models
{
    public class Products
    {
        #region Properties
        public List<Product> Items { get; } = new List<Product>();
        #endregion
    }
}