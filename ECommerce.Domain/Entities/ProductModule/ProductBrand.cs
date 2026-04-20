using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.ProductModule
{
    public class ProductBrand : BaseEntity<int>
    {
        public string Name { get; set; } = null!;

        #region Relationships

        // One To Many => ProductBrand - Product
        // make it with fluent API in the DbContext class
        //public ICollection<Product> Products { get; set; } = null!;

        #endregion
    }
}
