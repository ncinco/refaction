using System;
using Newtonsoft.Json;

namespace Xero.Web.Api.Models
{
    public class ProductOption
    {
        #region Properties
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        #endregion
    }
}