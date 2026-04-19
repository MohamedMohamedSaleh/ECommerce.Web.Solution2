using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace ECommerce.Web.Controllers
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m },
                new Product { Id = 2, Name = "Smartphone", Price = 499.99m },
                new Product { Id = 3, Name = "Headphones", Price = 199.99m }
            };
            return Ok(products);
        }

        // Get Product by id
        //[Route("{id}")]
        [HttpGet("{id}")]
        public IActionResult GetProduct(int? id)
        {
            if (id == null)
            {
                return BadRequest("Id is required");
            }
            Product? NewProduct = null;
            if (id == 1)
            {
                NewProduct = new Product { Id = 1, Name = "Laptop", Price = 999.99m };
            }
            else if (id == 2)
            {
                NewProduct = new Product { Id = 2, Name = "Smartphone", Price = 499.99m };

            }
            else if (id == 3)
            {
                NewProduct = new Product { Id = 3, Name = "Headphones", Price = 199.99m };
            }

            if (NewProduct != null)
            {
                return Ok(NewProduct);
            }
            return NotFound("Product not found");
        }

        // Create a new product
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null");
            }
            // In a real application, you would save the product to a database here
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // Update an existing product
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null || product.Id != id)
            {
                return BadRequest("Product is null or id mismatch");
            }
            // In a real application, you would update the product in the database here
            return NoContent();
        }

        // Delete a product
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            // In a real application, you would delete the product from the database here
            return NoContent();
        }
    }
}

    public class Product 
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}