// CrmProject.Api/Controllers/ProductController.cs

using CrmProject.Application.DTOs.ProductDtos;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Services.ServiceProducts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrmProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { message = $"ID'si {id} olan ürün bulunamadı." });
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDto createProductDto)
        {
            // Servis katmanından ValidationException fırlatılırsa,
            // bu yakalanmadığı için 500 Internal Server Error dönecektir.
            var addedProduct = await _productService.AddProductAsync(createProductDto);
            return CreatedAtAction(nameof(GetProductById), new { id = addedProduct.Id }, new { message = "Ürün başarıyla eklendi.", product = addedProduct });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            if (id != updateProductDto.Id)
            {
                return BadRequest(new { message = "URL'deki ID ile istek gövdesindeki ID uyuşmuyor." });
            }

            // Servis katmanından KeyNotFoundException veya ValidationException fırlatılırsa,
            // bu yakalanmadığı için 500 Internal Server Error dönecektir.
            await _productService.UpdateProductAsync(updateProductDto);
            return Ok(new { message = $"ID'si {id} olan ürün başarıyla güncellendi." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Önce ürünün varlığını kontrol et
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { message = $"ID'si {id} olan ürün bulunamadı. Silme yapılamadı." });
            }

            // Ürün varsa silme işlemini çağır
            await _productService.DeleteProductAsync(id);
            return Ok(new { message = $"ID'si {id} olan ürün başarıyla silindi." });
        }
    }
}
