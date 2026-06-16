using API_Example_ASP.Models;
using API_Example_ASP.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Example_ASP.Controllers;

[ApiController]
[Route("api-mobile/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpPost]
    public ActionResult<Product> CreateProduct([FromBody] CreateProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(new { errors });
        }

        var product = productService.AddProduct(request);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetProductById([FromRoute] string id)
    {
        if (!int.TryParse(id, out var productId) || productId <= 0)
        {
            return BadRequest(new
            {
                errors = new[] { "Id phải là số nguyên dương." }
            });
        }

        var product = productService.GetProductById(productId);
        if (product is null)
        {
            return NotFound(new
            {
                errors = new[] { $"Không tìm thấy sản phẩm với id = {productId}." }
            });
        }

        return Ok(product);
    }
}
