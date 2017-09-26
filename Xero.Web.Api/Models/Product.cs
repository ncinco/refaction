using System;
using Newtonsoft.Json;

namespace Xero.Web.Api.Models
{
    public class Product
    {
        #region Constructors
        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }
        #endregion

        #region Properties
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
        
        [JsonIgnore]
        public bool IsNew { get; }
        #endregion
    }
}