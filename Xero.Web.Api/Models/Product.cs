using System;

namespace Xero.Web.Api.Models
{
    public class Product
    {
        #region Properties
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
        #endregion
    }
}