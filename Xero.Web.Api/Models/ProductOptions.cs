using System.Collections.Generic;

namespace Xero.Web.Api.Models
{
    public class ProductOptions
    {
        #region Properties
        public List<ProductOption> Items { get; } = new List<ProductOption>();
        #endregion
    }
}