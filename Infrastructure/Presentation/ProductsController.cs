using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await serviceManager.ProductService.GetAllProductsAsyns();
            if (result == null)
                return BadRequest(); // 400
            return Ok(result); // 200
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await serviceManager.ProductService.GetProductByIdAsync(id);
            if (result == null)
                return NotFound(); // 404
            return Ok(result); // 200
        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await serviceManager.ProductService.GetAllBrandsAsync();
            if (result == null)
                return BadRequest();

            return Ok(result);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();
            if (result == null)
                return BadRequest();

            return Ok(result);
        }
    }
}
