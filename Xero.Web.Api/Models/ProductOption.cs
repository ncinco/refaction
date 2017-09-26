using System;
using Newtonsoft.Json;

namespace Xero.Web.Api.Models
{
    public class ProductOption
    {
        #region Constructors
        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }
        #endregion

        #region Properties
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }
        #endregion
    }
}