using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.ProductModule
{
    public class ProductType : BaseEntity<int>
    {
        public string Name { get; set; } = null!;

        #region Relationships

        // One To Many => ProductType - Product
        // make it with fluent API in the DbContext class
        //ICollection<Product> Products { get; set; } = null!;

        #endregion
    }
}
