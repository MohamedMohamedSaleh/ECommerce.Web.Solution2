using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.ProductModule
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!; 
        public decimal Price { get; set; }

        #region Relationships

        // Foreign Key => Navigation Property
        public int BrandId { get; set; }
        public ProductBrand ProductBrand { get; set; }

        // Foreign Key => Navigation Property
        public int TypeId { get; set; }
        public ProductType ProductType { get; set; }
        #endregion

    }
}
