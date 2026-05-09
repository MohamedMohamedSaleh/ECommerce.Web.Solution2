using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specification
{
    public class ProductSpecificationHelper
    {
        public static Expression<Func<Product, bool>> GetProductCriteria(ProductQueryParams productQuery)
        {
            return p => (!productQuery.BrandId.HasValue || p.BrandId == productQuery.BrandId) &&
                        (!productQuery.TypeId.HasValue || p.TypeId == productQuery.TypeId) &&
                        (string.IsNullOrEmpty(productQuery.Search) || p.Name.ToLower().Contains(productQuery.Search.ToLower()));
        }
    }
}
