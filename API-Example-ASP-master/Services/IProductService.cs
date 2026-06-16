using API_Example_ASP.Models;

namespace API_Example_ASP.Services;

public interface IProductService
{
    Product AddProduct(CreateProductRequest request);
    Product? GetProductById(int id);
}
