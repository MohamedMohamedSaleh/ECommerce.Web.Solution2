using ECommerce.Presentation.Attributes;
using ECommerce.ServiceAbstraction;
using ECommerce.Shared;
using ECommerce.Shared.DTOS.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        #region Get All Products
        // GET: baseurl/api/products?brandId && typeId (controller name is products)
        [HttpGet]
        [RedisCache]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _productService.GetAllProductsAsync(queryParams);
            if (products == null)
            {
                throw new KeyNotFoundException($"Products was not found");
            }
            return Ok(products);
        }
        #endregion

        #region Get Product By Id
        // GET: baseurl/api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            //throw new Exception();
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with id {id} was not found");
            }
            return Ok(product);
        }
        #endregion

        #region Get All Brands
        // GET: baseurl/api/products/brands
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var brands = await _productService.GetAllBrandsAsync();
            if(brands is null)
            {
                throw new KeyNotFoundException();
            }
            return Ok(brands);
        }
        #endregion

        #region Get All Types
        //Get: baseurl/api/products/types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var types = await _productService.GetAllTypesAsync();
            if(types is null)
            {
                throw new KeyNotFoundException();
            }
            return Ok(types);
        }
        #endregion

    }
}
